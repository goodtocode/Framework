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