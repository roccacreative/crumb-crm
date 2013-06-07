$(document).ready(function () {

    // msgs
    $('.msg-close').click(function(){
        $('.msg').remove();
    });

    Modernizr.load();

    // lazy loading
    initTriggerView();

    var current_controller;
    var lead_type;

    function initTriggerView() {
        //get current controller from url

        //2 segment urls
        var match = /[^.]([\w]+)\/([\w]+)/.exec(window.location.pathname);
        if (match != null) {
            current_controller = match[1];
            lead_type = match[2];
        }
        else { //1 seg
            match = /[^.]([\w]+)/.exec(window.location.pathname);
            current_controller = match[1];
        }
 

        $(".content-loading").triggerOnView({
            debug: false,      //displays debugging info in the browser
            eventType: '',    //type of the event that should be triggered on the targeted element
            verticalRange: 0,          //vertical offset to expand reaction area (you may need to trigger event even
            //before element gets into the view, but user is in the vicinity of it)
            horizontalRange: 0,          //horizontal offset to expand reaction area
            singleShotOnly: true,       //true if event should be triggered only once when element become visible
            callback: function (elem, settings) {

                if (current_controller.slice(-1) == 's') {
                    current_controller = current_controller.substring(0, current_controller.length - 1)
                }

                if (match[2] != null) {
                    var url = '/' + current_controller + '/renderitems/' + lead_type + '?page=' + $(elem).data('next')
                } else {
                    var url = '/' + current_controller + '/renderitems?page=' + $(elem).data('next')
                }
                $.get(url, function (data) {
                    $('.content-loading').remove();
                    $('.stream-content').append(data);
                    initTriggerView();
                });

            } 
        });
    }

    $('.convert').click(function (e) {
        //ensure lead has a company
        var name = $(this).parent().parent().find('.company-name').html();
        if (name == null || name.length == 0) {
            e.preventDefault();
            $('.msg').remove();
            $('h1').after('<p class="msg msg-error">Lead you\'re attempting to convert is missing a company property, please add one to continue<a href="javascript:;" class="msg-close right">close</a></p>');
        }
    });

    // date picker
    var datevalue = $(".datepicker").attr('value');
    $(".datepicker").datepicker();
    $(".datepicker").datepicker("option", "dateFormat", "DD, d MM, yy");
    $(".datepicker").datepicker('setDate', datevalue);

    // toggle add new company form input row (contact/add)
    $('.add-company').click(function () {
        $('.new-company-row').removeClass('nodisplay').hide();
        $('.new-company-row').slideDown();
        $(this).hide();
    });

    $('.add-campaign').click(function () {
        $('.new-campaign-row').removeClass('nodisplay').hide();
        $('.new-campaign-row').slideDown();
        $(this).hide();
    });

	// truncated text "more" link
	$('.item span').each(function(){
		if($(this).width() > 600) {
			var short_text = $(this).html();
			var long_text = short_text
			$(this).attr('data-long-text', long_text)
			
     		short_text = short_text.trim().substring(0, 60)
        	.split(" ")
        	.slice(0, -1)
        	.join(" ")
        	$(this).attr('data-short-text', short_text);
			$(this).html(short_text + '...<a href="javascript:;" class="toggle-more">more</a>');
		}
	});

    // asc/desc listings sorting
	$('.sorting').change(function () {
	    loc = window.location + '' //ensure string type
	    loc = clear_url(loc);

	    if (loc != null) { //if theres a query string
	        window.location = loc + '/' + $(this).val();
	    } else {
	        window.location = window.location + '/' + $(this).val();
	    }
	});


	function clear_url(url) {
        //remove any query strings
	    url = url.split('?')[0]
	    //remove any /asc /desc filtering
	    url = url.replace('/asc', '');
	    url = url.replace('/desc', '');
	    return url;
	}
	

    //validate the forms..
	$("form.box").validate();

    // text truncation, used on notes listings
	$('.toggle-more').live('click',function(){
	    var txt = $(this).parent();
		if(txt.width() > 600) {
			$(this).parent().html($(this).parent().data('short-text') + '...<a href="javascript:;" class="toggle-more">more</a>')
		} else {
			$(this).parent().html($(this).parent().data('long-text') + '...<a href="javascript:;" class="toggle-more">less</a>')
		}
	});

    /* item checkboxes, multi-del */
	$('.item input[type=checkbox]').change(function () {
	    var parent_checkbox = $('.stream-head input[type=checkbox]').parent();
	    if ($(this).is(":checked")) {
	        parent_checkbox.next('.btn-check-delete').hide();
	        parent_checkbox.next('.btn-check-delete').removeClass('nodisplay').show();
	        $(this).parent().removeClass('hidden');
	    } else {
	        //
	        if ($('.item input[type=checkbox]:checked').length == 0) {
	            parent_checkbox.next('.btn-check-delete').hide();
	        }
           
	        $(this).parent().addClass('hidden');
	    }
	});

    // listings checkboxes
	$('.stream-head input[type=checkbox]').click(function () {
	    var checkbox = $('.stream-content').find('input[type=checkbox]');

	    checkbox.parent().removeClass('hidden');

	    if ($(this).is(":checked")) {
	        $(this).parent().next('.btn-check-delete').hide();
	        checkbox.attr('checked', 'checked').parent().show();
	        $(this).parent().next('.btn-check-delete').removeClass('nodisplay').show();
	    } else {
	        $(this).parent().next('.btn-check-delete').hide();
	        checkbox.removeAttr('checked').parent().addClass('hidden');
	    }
	});


    //stream search 
	$('#stream-search input[type=text]').keydown(function () {
	    var q = this.value;
	    if (q.length == 1) {
	        $('.highlight').addClass('highlight');
	    } else if ((q.length+1) > 2) {
	        $(this).parent().parent().parent().find('.stream-content .item').each(function () {
	            var item = $(this).find('.txt-large').text();
	            if (item.toLowerCase().indexOf(q) >= 0) {
	                $(this).show();
	                $(this).unhighlight();
	                $(this).highlight(q);
	            } else {
	                $(this).hide();
	            }

	        });
	    } else {
	        $(this).parent().parent().parent().find('.stream-content .item').unhighlight();
	        $(this).parent().parent().parent().find('.stream-content .item').show();
	    }
	});

    // listings checkbox control delete
	$('.btn-check-delete').live('click', function () {
	    var action = $(this).data('action');

	    $('.stream-content .item').find('form input[type=checkbox]:checked').each(function() {
	        id = $(this).parent().parent().parent().data('id');
	        $.post(action + id);
	    
	    });

	    $('.stream-content').find('input[type=checkbox]:checked').closest('.item').slideUp(200, function () {
	        $('.stream-content').find('input[type=checkbox]:checked').closest('.item').remove();
	    });
	});


    /* stream sorting js dropdown asc/desc */
    // TODO selected status on page load
	$('.inline-select > select').customSelect({ customClass: 'inline-select-link' });

    // list item tooltips
	$('a[title]').qtip({
				position: {
					my: 'top center', // Use the corner...
					at: 'bottom center' // ...and opposite corner
				},
				
				style: {
					classes: 'qtip-shadow qtip-dark qtip-rounded'
				}
			});

    // sale/lead draggable control
	$(".state").draggable({
	    appendTo: 'body',
	    containment: 'window',
	    scroll: false,
	    helper: 'clone',
	    drag: function () {
	        $('.drop-stages').fadeIn();
	    },
	    stop: function () {
	        if ($('.ui-state-highlight').length == 0) {
	            $('.drop-stages').fadeOut();
	        }
	    }
	});


    // invoice modal
	$('.generate-invoice').click(function (e) {
	    e.preventDefault();
	    var item = $(this).parent().parent();
	    var quote_id = item.data('id');
	    var title = item.data('title');

	    $.blockUI({
	        message: $('.quotes-popup'), css: { padding: '10px', top: '20%' }, onBlock: function () {
	            $('.quotes-popup #QuoteID').val(quote_id);
	            $('.quotes-popup #Title').val(title);
	        }
	    });
	});


    // drag n drop sale/lead status updating
	$( ".drop-stage" ).droppable({
		drop: function( event, ui ) {
			$(this)
			.addClass( "ui-state-highlight" )
			.find( "p:last-child" )
			.html( "Stage moved!" );			

			var dropItem = $(this);

			var next = $(this).data('stage');

			var stage_id = $(this).data('stage-id');
			var next_target = $('.stages-nav a[data-stage=' + next + ']');
			var next_stage_id = next_target.data('stage-id');
			
			var url = window.location.pathname;
			url = url.charAt(url.length - 1);
			var prev_id = url;

		    //error if the target doesn't exist at either side of the drag
			if(next.length < 1 || next_target.length < 1) {
				//alert('An error occurred, please try again')
			}else {
				var current = next_target.find('.total').html();
				//$('.stages-nav a[data-stage='+next+'] .total').html(parseInt(current)+1);
				$(ui.draggable).closest('.item').slideUp();

				var update_id = $(ui.draggable).closest('.item').data('id');
				var update_type = $(ui.draggable).closest('.item').data('type');

				$.get('/' + update_type + '/update/' + update_id + '?type=' + stage_id, function (data) {
				
				})
                .fail(function () { alert('An error occurred, please try again'); })
                .done(function () {                    
                    var navitem = $('.stages-nav a[data-stage-id=' + prev_id + '] .total');

                    var newtotal = parseInt(navitem - prev_id);
                    if (isNaN(newtotal) || newtotal == 'NaN') {
                        newtotal = 0;
                    }
                    navitem.text(newtotal);

                    //flash drop div
                    var drop = $('.drop-stage[data-stage=' + next + ']')
                    var current_bg = drop.css('backgroundColor');
                    drop.animate({ backgroundColor: '#d9d1ba' }, 300 / 2);
                    drop.animate({ backgroundColor: current_bg }, 400 / 2, function () {
                        $('.notes-popup').find('#ItemID').val(update_id);
                          $.blockUI({
                              message: $('.notes-popup'), css: { padding: '10px', top: '20%' }, onBlock: function () {
                            
  
                                  $('.notes-popup textarea').keydown(function (e) {
                                      if (e.ctrlKey && e.keyCode == 13) {
                                          $(this).closest('form').submit();
                                      }
                                  })
                              }
                          });
                        $('.drop-stages').fadeOut();
                        dropItem.removeClass("ui-state-highlight")
                    });

                    //inc totals
                    $('.stages-nav a[data-stage=' + next + '] .total').html(parseInt(current) + 1);

                    //dec totals
                    var old_current = $('.stages-nav a.active').find('.total').text();

                    var newtotal = parseInt(old_current - 1);
                    if (isNaN(newtotal) || newtotal == 'NaN') {
                        newtotal = 0;
                    }
                    $('.stages-nav a.active').find('.total').html(newtotal);
                })
			}
		}
	});

	$(document).on('click', '.blockUI.blockOverlay', function () {
	    $.unblockUI();
	});

    // filter by tag
	$('#TagSelector').tokenInput("/tags/gettags", {
	    preventDuplicates: true,
	    allowFreeTagging: true,
	    allowCustomEntry: true,
	    tokenValue: 'name',
	    prePopulate: $('#TagPrePopulate').length > 0 ? JSON.parse($('#TagPrePopulate').val()) : ''
	});




}); //end doc

var events = {
    noteSuccess: function () {
        $.unblockUI();
    },
    quoteSuccess: function () {
        $.unblockUI();
        window.location.replace("/invoice");
    }
};

var App = function () {

    // IE mode
    var isRTL = false;
    var isIE8 = false;
    var isIE9 = false;
    var isIE10 = false;

    var responsiveHandlers = [];

    // theme layout color set
    var layoutColorCodes = { 'blue': '#4b8df8', 'red': '#e02222', 'green': '#35aa47', 'purple': '#852b99', 'grey': '#555555', 'light-grey': '#fafafa', 'yellow': '#ffb848' };

    return {
        getLayoutColorCode: function (name) {
            if (layoutColorCodes[name]) {
                return layoutColorCodes[name];
            } else {
                return '';
            }
        }
    }
}();