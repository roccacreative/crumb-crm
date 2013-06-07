/**
 * With the help of this plugin you can achieve similar functionality as you can see on Twitter infinitely loading your
 * wall contents.
 *
 * Plugin will help you automatically trigger standard event when specified DOM element gets into the user view (ie. end
 * of your content). Usage:
 *
 * $("#myElement").triggerOnView();
 * $("#myElement").triggerOnView({
 *       debug           : false,      //displays debugging info in the browser
 *       eventType       : 'click',    //type of the event that should be triggered on the targeted element
 *       verticalRange   : 0,          //vertical offset to expand reaction area (you may need to trigger event even
 *                                     //before element gets into the view, but user is in the vicinity of it)
 *       horizontalRange : 0,          //horizontal offset to expand reaction area
 *       singleShotOnly  : true,       //true if event should be triggered only once when element become visible
 *       callback        : null        //function that should be invoked just before event triggering, if the function
 *                                     //returns false, triggering is stopped
 *                                     //function signature: function(domElement, settings)
 * });
 *
 * Licensed under the MIT License.
 * Homepage: http://jquery.novoj.net/triggerOnView/index.html
 *
 * @version 1.0 (4. 6. 2011)
 * @author Jan Novotny (http://blog.novoj.net)
 */
(function ($) {
    $.fn.triggerOnView = function (options) {

        var settings = {
            debug: false,
            eventType: 'click',
            verticalRange: 0,
            horizontalRange: 0,
            singleShotOnly: true,
            callback: null
        };

        return this.each(function () {

            var $this = $(this);

            if (options) {
                $.extend(settings, options);
            }

            var triggerOnView = function () {
                var offset = $this.offset();
                var $window = $(window);
                var doTrigger = $window.scrollTop() + $window.height() >= offset.top + $this.height() + settings.verticalRange &&
                                $window.scrollTop() <= offset.top + settings.verticalRange &&
                                $window.scrollLeft() + $window.width() >= offset.left + $this.width() + settings.horizontalRange &&
                                $window.scrollLeft() <= offset.left + settings.horizontalRange;
                if (settings.debug) {
                    var $monitor = $('#monitor');
                    var debugContent = "<b>Offset top:</b> " + offset.top + "<br><b>ScrollTop:</b> " + $window.scrollTop() +
                        "<br><b>Offset left:</b> " + offset.left + "<br><b>ScrollLeft:</b> " + $window.scrollLeft() +
                        "<br><b>ScrollTop + Window:</b> " + ($window.scrollTop() + $window.height()) +
                        "<br><b>ScrollLeft + Window:</b> " + ($window.scrollLeft() + $window.width()) +
                        "<br><b>Will trigger:</b> " + doTrigger;
                    if ($monitor.length == 0) {
                        $(document.body).append("<div id='monitor' style='position: absolute; top: 10px; left: 10px; padding: 1em; color: white; background-color: black'>" + debugContent + "</div>");
                    } else {
                        $monitor.css("top", window.pageYOffset + 10 + "px").html(debugContent);
                    }
                }
                if (doTrigger) {
                    var progress = true;
                    if ($.isFunction(settings.callback)) {
                        if (settings.callback($this, settings) === false) {
                            progress = false;
                        }
                    }
                    if (progress) {
                        $this.trigger(settings.eventType);
                        //it's one shot function, so unregister itself
                        if (settings.singleShotOnly) {
                            $window.unbind("scroll", arguments.callee);
                        }
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    return false;
                }
            };

            if (!triggerOnView.call(this)) {
                $(window).bind("scroll", triggerOnView);
            }

        });

    };
})(jQuery);