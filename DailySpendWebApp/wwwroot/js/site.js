
//function ShowSnackBar(Message, Type) {

//    var x = document.getElementById("SnackBarMessage");

//    if (Message == 'BillCreated')
//    {
//        x.innerHTML = "<div><strong>Congrats</strong> you set up <strong>" + '@TempData["BillName"]' + "</strong> as a Bill for <strong>£" + '@TempData["BillAmount"]' + "</strong> which will require you to save <strong>£" + '@TempData["BillDailyValue"]' + "</strong> a day!</div>"
//    }
//    else if (Message == 'SavingCreated')
//    {
//        x.innerHTML = "<div><strong>Congrats</strong> you set up <strong>" + '@TempData["SavingsName"]' + "</strong> as a Bill for <strong>£" + '@TempData["SavingsGoal"]' + "</strong> which will required you to save <strong>£" + '@TempData["RegularSavingValue"]' + "</strong> a day!</div>";
//    }
//    else if (Message == 'SavingCancelled')
//    {
//        x.innerHTML = '<div>Youve back out of creating the <strong>Saving</strong>, Whats wrong you scared? Dont worry we believe in you!</div>'
//    }
//    else if (Message == 'BillCancelled')
//    {
//        x.innerHTML = '<div>You <strong>Deleted</strong> your Bill, This doesnt delete it from life though just in the app it still needs to be paid, so maybe just add it anyway?</div>';
//    }

//    var y = document.getElementById("snackbar");
//    y.className = y.className.replace("alert-success", "");
//    y.className = y.className.replace("alert-danger", "");

//    if (Type == 'success') {
//        y.className = y.className + ' alert-success';
//    }
//    else if (Type == 'danger') {
//        y.className = y.className + ' alert-danger';
//    }

//    y.className = y.className + " show";

//    setTimeout(function () { x.className = x.className.replace("show", ""); }, 10900);
//}

 function FormatBudgetSettingsDate (dateObject, seperator, DatePattern, IncludeDay) {

    const monthFull = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    const weekdayFull = ["Sunday", "Monday", "Tueday", "Wednesday", "Thursday", "Friday", "Saturday"];
    const monthShort = ["Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec"];
    const weekdayShort = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];

    var DatePattern = DatePattern.toLowerCase();
    var d = new Date(dateObject);

    var weekday = d.getDay();
    if (IncludeDay == "Full")
    {
        weekday = weekdayFull[weekday]
    }
    else if (IncludeDay == "Short")
    {
        weekday = weekdayShort[weekday]
    }

    var month = d.getMonth();
    if (DatePattern.includes("mmmm"))
    {
        month = monthFull[month]
    }
    else if (DatePattern.includes("mmm"))
    {
        month = monthShort[month]
    }
    else if (DatePattern.includes("mm"))
    {
        month = month + 1
        if (month < 10)
        {
            month = "0" + month;
        }
        else
        {
            month.toString()
        }
    }

    var year = d.getFullYear().toString();
    if (DatePattern.includes("yyyy"))
    {
        var year = year
    }
    else if (DatePattern.includes("yy"))
    {
        var year = year.slice(- 2)
    }

    var day = d.getDate();
    if (day < 10)
    {
        day = "0" + day;
    }

    var CurrentString = ""
    if (DatePattern.charAt(0) == 'y') {
        var CurrentString = "y"
        var First = year
    }
    else if (DatePattern.charAt(0) == 'd') {
        var CurrentString = "d"
        var First = day
    }
    else if (DatePattern.charAt(0) == 'm') {
        var CurrentString = "m"
        var First = month
    }

    j=2
    for (var i = 1; i < DatePattern.length; i++)
    {

        if (DatePattern.charAt(i) != CurrentString)
        {
            if (DatePattern.charAt(i) == 'y') {
                var CurrentString = "y"
                if (j == 2)
                {
                    var Second = year
                }
                else if (j == 3)
                {
                    var Third = year
                }
                
            }
            else if (DatePattern.charAt(i) == 'd') {
                var CurrentString = "d"
                if (j == 2)
                {
                    var Second = day
                }
                else if (j == 3)
                {
                    var Third = day
                }
            }
            else if (DatePattern.charAt(i) == 'm') {
                var CurrentString = "m"
                if (j == 2)
                {
                    var Second = month
                }
                else if (j == 3)
                {
                    var Third = month
                }
            }

            j++
        }
    }


    var date = First + seperator + Second + seperator + Third

    if (IncludeDay != "None")
    {
        var date = weekday + "  " + date;
    }


    return date;
};



function ShowHideInfo(DivName, Icon) {
    var x = document.getElementById(DivName);
    var y = document.getElementById(Icon);

    if (x.style.display === "none") {
        x.style.display = "flex";
        y.className = "bi bi-arrow-bar-left text-white";
    } else {
        x.style.display = "none";
        y.className = "bi bi-arrow-bar-right text-white";
    }
}

$("input[data-type='NegativeCurrency']").on({
    keyup: function () {
        NegativeformatCurrency($(this));
    },
    blur: function () {
        NegativeformatCurrency($(this), "blur");
    }
});

$("input[data-type='PositiveCurrency']").on({
    keyup: function () {
        formatCurrency($(this));
    },
    blur: function () {
        formatCurrency($(this), "blur");
    }
});

$("input[data-type='currency']").on({
    keyup: function () {
        formatCurrency($(this));
    },
    blur: function () {
        formatCurrency($(this), "blur");
    }
});

function formatNumber(n) {
    // format number 1000000 to 1,234,567
    if (typeof CurrencySpacer == 'undefined') {
        var CurrencySpacer = ","
    }

    return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, CurrencySpacer)
}


function formatCurrency(input, blur) {
    // appends $ to value, validates decimal side
    // and puts cursor back in right position.
    if (typeof CurrencySymbol == 'undefined') {
        var CurrencySymbol = "£"
    }

    if (typeof CurrencyPlacement == 'undefined') {
        var CurrencyPlacement = "Before"
    }

    if (typeof SymbolSpace == 'undefined') {
        var SymbolSpace = "No"
    }

    if (typeof DecimalSeperator == 'undefined') {
        var DecimalSeperator = "."
    }



    // get input value
    var input_val = input.val();

    // don't validate empty input
    if (input_val === "") { return; }

    // original length
    var original_len = input_val.length;

    // initial caret position 
    var caret_pos = input.prop("selectionStart");

    // check for decimal
    if (input_val.indexOf(".") >= 0) {

        // get position of first decimal
        // this prevents multiple decimals from
        // being entered
        var decimal_pos = input_val.indexOf(".");

        // split number by decimal point
        var left_side = input_val.substring(0, decimal_pos);
        var right_side = input_val.substring(decimal_pos);

        // add commas to left side of number
        left_side = formatNumber(left_side);

        // validate right side
        right_side = formatNumber(right_side);

        // On blur make sure 2 numbers after decimal
        if (blur === "blur") {
            right_side += "00";
        }

        // Limit decimal to only 2 digits
        right_side = right_side.substring(0, 2);

        // join number by .
        if (CurrencyPlacement == "Before") {
            if (SymbolSpace == "No") {
                input_val = CurrencySymbol + left_side + DecimalSeperator + right_side;
            }
            else if (SymbolSpace == "Yes") {
                input_val = CurrencySymbol + " " + left_side + DecimalSeperator + right_side;
            }
        }
        else if (CurrencyPlacement == "After") {
            if (SymbolSpace == "No") {
                input_val = left_side + DecimalSeperator + right_side + CurrencySymbol;
            }
            else if (SymbolSpace == "Yes") {
                input_val = left_side + DecimalSeperator + right_side + " " + CurrencySymbol;
            }
        }

    } else {
        // no decimal entered
        // add commas to number
        // remove all non-digits
        input_val = formatNumber(input_val);

        if (CurrencyPlacement == "Before") {
            if (SymbolSpace == "No") {
                input_val = CurrencySymbol + input_val + DecimalSeperator;
            }
            else if (SymbolSpace == "Yes") {
                input_val = CurrencySymbol + " " + input_val + DecimalSeperator;
            }
        }
        else if (CurrencyPlacement == "After") {
            if (SymbolSpace == "No") {
                input_val = input_val + DecimalSeperator + CurrencySymbol;
            }
            else if (SymbolSpace == "Yes") {
                input_val = input_val + DecimalSeperator + " " + CurrencySymbol;
            }
        }

        // final formatting
        if (blur === "blur") {
            left_side = formatNumber(input_val);
            right_side = DecimalSeperator + "00"
            if (CurrencyPlacement == "Before") {
                if (SymbolSpace == "No") {
                    input_val = CurrencySymbol + left_side + DecimalSeperator + right_side;
                }
                else if (SymbolSpace == "Yes") {
                    input_val = CurrencySymbol + " " + left_side + DecimalSeperator + right_side;
                }
            }
            else if (CurrencyPlacement == "After") {
                if (SymbolSpace == "No") {
                    input_val = left_side + DecimalSeperator + right_side + CurrencySymbol;
                }
                else if (SymbolSpace == "Yes") {
                    input_val = left_side + DecimalSeperator + right_side + " " + CurrencySymbol;
                }
            }
        }
    }

    // send updated string to input
    input.val(input_val);

    // put caret back in the right position
    var updated_len = input_val.length;
    caret_pos = updated_len - original_len + caret_pos;
    input[0].setSelectionRange(caret_pos, caret_pos);
}

function NegativeformatCurrency(input, blur) {
    if (document.getElementById('CurrencySymbol').value == '') {
        var CurrencySymbol = "£"
    }
    else {
        var CurrencySymbol = document.getElementById('CurrencySymbol').value
    }

    if (typeof CurrencyPlacement == 'undefined') {
        var CurrencyPlacement = "Before"
    }

    if (typeof SymbolSpace == 'undefined') {
        var SymbolSpace = "No"
    }

    if (typeof DecimalSeperator == 'undefined') {
        var DecimalSeperator = "."
    }

    // get input value
    var input_val = input.val();

    // don't validate empty input
    if (input_val === "") { return; }

    // original length
    var original_len = input_val.length;

    // initial caret position 
    var caret_pos = input.prop("selectionStart");

    // check for decimal
    if (input_val.indexOf(DecimalSeperator) >= 0) {

        // get position of first decimal
        // this prevents multiple decimals from
        // being entered
        var decimal_pos = input_val.indexOf(DecimalSeperator);

        // split number by decimal point
        var left_side = input_val.substring(0, decimal_pos);
        var right_side = input_val.substring(decimal_pos);

        // add commas to left side of number
        left_side = formatNumber(left_side);

        // validate right side
        right_side = formatNumber(right_side);

        // On blur make sure 2 numbers after decimal
        if (blur === "blur") {
            right_side += "00";
        }

        // Limit decimal to only 2 digits
        right_side = right_side.substring(0, 2);

        // join number by .
        if (CurrencyPlacement == "Before") {
            if (SymbolSpace == "No") {
                input_val = "-" + CurrencySymbol + left_side + DecimalSeperator + right_side;
            }
            else if (SymbolSpace == "Yes") {
                input_val = "-" + CurrencySymbol + " " + left_side + DecimalSeperator + right_side;
            }
        }
        else if (CurrencyPlacement == "After") {
            if (SymbolSpace == "No") {
                input_val = left_side + DecimalSeperator + right_side + CurrencySymbol + "-";
            }
            else if (SymbolSpace == "Yes") {
                input_val = left_side + DecimalSeperator + right_side + " " + CurrencySymbol + "-";
            }
        }

    } else {
        // no decimal entered
        // add commas to number
        // remove all non-digits
        input_val = formatNumber(input_val);

        if (CurrencyPlacement == "Before") {
            if (SymbolSpace == "No") {
                input_val = "-" + CurrencySymbol + input_val + DecimalSeperator;
            }
            else if (SymbolSpace == "Yes") {
                input_val = "-" + CurrencySymbol + " " + input_val + DecimalSeperator;
            }
        }
        else if (CurrencyPlacement == "After") {
            if (SymbolSpace == "No") {
                input_val = input_val + DecimalSeperator + CurrencySymbol + "-";
            }
            else if (SymbolSpace == "Yes") {
                input_val = input_val + DecimalSeperator + " " + CurrencySymbol + "-";
            }
        }

        // final formatting
        if (blur === "blur") {
            left_side = formatNumber(input_val);
            right_side = DecimalSeperator + "00"
            if (CurrencyPlacement == "Before") {
                if (SymbolSpace == "No") {
                    input_val = "-" + CurrencySymbol + left_side + DecimalSeperator + right_side;
                }
                else if (SymbolSpace == "Yes") {
                    input_val = "-" + CurrencySymbol + " " + left_side + DecimalSeperator + right_side;
                }
            }
            else if (CurrencyPlacement == "After") {
                if (SymbolSpace == "No") {
                    input_val = left_side + DecimalSeperator + right_side + CurrencySymbol + "-";
                }
                else if (SymbolSpace == "Yes") {
                    input_val = left_side + DecimalSeperator + right_side + " " + CurrencySymbol + "-";
                }
            }
        }
    }

    // send updated string to input
    input.val(input_val);

    // put caret back in the right position
    var updated_len = input_val.length;
    caret_pos = updated_len - original_len + caret_pos;
    input[0].setSelectionRange(caret_pos, caret_pos);
}


