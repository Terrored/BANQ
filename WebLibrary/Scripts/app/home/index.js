$(document).ready(function () {
    $(".d-none").removeClass("d-none");

    $(".slick-slider").slick({
        infinite: true,
        slidesToShow: 3,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 5000,
        dots: true,
        pauseOnFocus: false,
        pauseOnHover: false,

    }).
        on("setPosition", function (event, slick) {
            $(this).find('.slide').height('auto');
            var slickTrack = $(this).find('.slick-track');
            var slickTrackHeight = $(slickTrack).height();
            $(this).find('.slide').css('height', slickTrackHeight + 'px');
        });
});