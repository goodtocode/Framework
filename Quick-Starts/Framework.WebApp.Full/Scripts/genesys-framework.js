//
// Sets element z-index to max
//
$.toMaxZIndex = $.fn.toMaxZIndex = function (opt) {
    var def = { inc: 10, group: "*" };
    var maxValue = 0;
    var current;
    $.extend(def, opt);
    $(def.group).each(function () {
        current = parseInt($(this).css('z-index'));
        maxValue = current > maxValue ? current : maxValue;
    });
    if (!this.jquery)
        return maxValue;
    return this.each(function () {
        maxValue += def.inc;
        $(this).css("z-index", maxValue);
    });
}


//
// Date validation and formatting
//
function FormatDate(id) {
    var keypressed;
    if (event.which) keypressed = event.which;
    if (event.keyCode) keypressed = event.keyCode;
    if (!((keypressed === 8) || (keypressed === 9) || (keypressed === 12) || (keypressed === 13) || (keypressed === 32) || ((keypressed >= 35) && (keypressed <= 39)) || ((keypressed >= 45) && (keypressed <= 57)) || ((keypressed >= 96) && (keypressed <= 105)))) return false;
    if (id != null && keypressed != null) {
        if (keypressed !== '8')
            var control = document.getElementById(id);
        if (control != null) {
            if (control.value.length === 2 | control.value.length === 5) {
                var text = control.value + "/";
                control.value = text;
            }
        }
    }
    return null;
}

function IsValidDate(dateValue, allowFuture) {
    var returnValue = false;
    var dd = new Date();
    var temp = new Array();
    temp = dateValue.split('/');
    if (temp.length === 3) {

        if (allowFuture === true) {
            if (year > dd.getFullYear()) {
                returnValue = false;
            }
            if (year === dd.getFullYear()) {
                if (month - 1 > dd.getMonth()) {
                    returnValue = false;
                }
                if (month === dd.getMonth() + 1) {
                    if (day > dd.getDate()) {
                        returnValue = false;
                    }
                }
            }
        }

        if ((month === 2) && (day > 29))
            returnValue = false;
        else if ((month === 2) && (day === 29) && (year % 4 !== 0))
            returnValue = false;
        else if ((day === 31) && ((month === 4) || (month === 6) || (month === 9) || (month === 11)))
            returnValue = false;
        else if ((month > 0) && (month <= 12) && (day > 0) && (day <= 31) && (year > 1901))
            returnValue = true;
    }

    return returnValue;
}
