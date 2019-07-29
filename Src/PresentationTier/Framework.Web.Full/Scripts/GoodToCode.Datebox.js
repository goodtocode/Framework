(function ($) {
    $.fn.Datebox = function (options) {
        var settings = $.extend({
            ColorError = 'red',
            ColorSuccess = 'black',
            DateSeparator = '/'
        }, options);

        // Attach datebox to DOM element
        this.onkeydown = FormatDate(this, settings.separator);
        this.onchange = ValidateDate(this, settings.ColorError, settings.ColorSuccess, settings.separator);

        // Allow chainable
        return this;
    }

    function ValidateDate(dateControl, colorError, colorSuccess, separator) {
        if (!IsValidDate(dateControl.value, separator))
            dateControl.style.color = colorError;
        else
            dateControl.style.color = colorSuccess;
    }

    function IsValidDate(dateValue, separator) {
        var returnValue = false;
        var temp = dateValue.split(separator);

        if (temp.length === 3) {
            var month = Number(temp[0]);
            var day = Number(temp[1]);
            var year = Number(temp[2]);

            if ((month === 2) && (day > 29))
                returnValue = false;
            else if ((month === 2) && (day === 29) && (!IsLeapYear(year)))
                returnValue = false;
            else if ((day === 31) && ((month === 4) || (month === 6) || (month === 9) || (month === 11)))
                returnValue = false;
            else if ((month > 0) && (month <= 12) && (day > 0) && (day <= 31))
                returnValue = true;
            else
                returnValue = false;
        }

        return returnValue;
    }
   

    function IsLeapYear(year) {
        var returnValue = false;
        if (((year % 4 === 0) && (year % 100 !== 0)) || (year % 400 === 0))
            returnValue = true;
        return returnValue;
    }

    function FormatDate(dateControl, separator) {
        var keypressed;

        if (event.which) keypressed = event.which;
        if (event.keyCode) keypressed = event.keyCode;
        if (!(keypressed === 8 || keypressed === 9 || keypressed === 12 || keypressed === 13 || keypressed === 32
            || (keypressed >= 35 && keypressed <= 39) || (keypressed >= 45 && keypressed <= 57)
            || (keypressed >= 96 && keypressed <= 105)))
            return false;
        if (dateControl !== undefined && dateControl !== null && keypressed !== null) {
            if (keypressed !== 8) {
                if (dateControl.value.length === 2 | dateControl.value.length === 5) {
                    var text = dateControl.value + separator;
                    dateControl.value = text;
                }
            }
        }

        return true;
    }
}(jQuery));
