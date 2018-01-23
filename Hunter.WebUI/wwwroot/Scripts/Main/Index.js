$(function () {
    $(".navbar-expand-toggle").click(function () {
        var $sideMenu = $(".side-menu")
        if ($sideMenu.width() == 250) {
            $sideMenu.next().css("left", "60px")
        } else {
            $sideMenu.next().css("left", "250px") 
        }
        $(".app-container").toggleClass("expanded");
        return $(".navbar-expand-toggle").toggleClass("fa-rotate-90");
    });
    return $(".navbar-right-expand-toggle").click(function () {
        $(".navbar-right").toggleClass("expanded");
        return $(".navbar-right-expand-toggle").toggleClass("fa-rotate-90");
    });
});

$(function () {
    return $(".side-menu .nav .dropdown").on('show.bs.collapse', function () {
        return $(".side-menu .nav .dropdown .collapse").collapse('hide');
    });
});
