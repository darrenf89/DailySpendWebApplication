$("input[data-type='currency']").on({
    keyup: function () {
        formatCurrency($(this));
    },
    blur: function () {
        formatCurrency($(this), "blur");
    }
});

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

function formatNumber(n) {
    // format number 1000000 to 1,234,567
    return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",")
}

function ShowHideInfo(DivName, Icon) {
    var x = document.getElementById(DivName);
    var y = document.getElementById(Icon);

    if (x.style.display === "none") {
        x.style.display = "block";
        y.className = "bi bi-arrow-bar-left text-white";
    } else {
        x.style.display = "none";
        y.className = "bi bi-arrow-bar-right text-white";
    }
}


function formatCurrency(input, blur) {
    // appends $ to value, validates decimal side
    // and puts cursor back in right position.

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
        input_val = "£" + left_side + "." + right_side;

    } else {
        // no decimal entered
        // add commas to number
        // remove all non-digits
        input_val = formatNumber(input_val);
        input_val = "£" + input_val;

        // final formatting
        if (blur === "blur") {
            input_val += ".00";
        }
    }

    // send updated string to input
    input.val(input_val);

    // put caret back in the right position
    var updated_len = input_val.length;
    caret_pos = updated_len - original_len + caret_pos;
    input[0].setSelectionRange(caret_pos, caret_pos);
}
