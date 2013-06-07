if (!$('.no-mobile').is(":visible")) {

    //hide the desktop stages navigation
    $('.stages-nav').hide();

    //TODO set selected nav
    //$('.select-stages-nav option:selected', 'select').removeAttr('selected')
    //$('.select-stages-nav').find('option[value=emailed]').attr('selected', 'selected');

    $('.show-nav').click(function () {
        $('header .nav').toggle();
        $('.sub-nav').toggle();
        $('header .inner > .right').toggle();
    });

   
    // popular select control from desktop stages navigation
    var stages = [];
    $('.stages-nav > a').each(function () {
        var t = $(this).find('.total').text();
        $(this).find('.total').remove()
        stages.push($(this).text() + " (" + t + ")");
    });

    items = "";
    for (var i = 0; i < stages.length; i++) {
        items += "<option value=\"" + stages[i].replace(" ", "").toLowerCase() + "\">" + stages[i] + "</option>";
    }

    $('.stages-nav').after(
        "<p>Select Lead Stage:</p>" +
        "<select class=\"select-stages-nav\">" +
        items +
        "</select>"
    );


    // stage navigation
    $('.select-stages-nav').change(function () {
        var s = $(this).find(":selected").val();
        s = s.substring(0, s.indexOf('('));
        window.location = '/leads/' + s;
    });



} //end
