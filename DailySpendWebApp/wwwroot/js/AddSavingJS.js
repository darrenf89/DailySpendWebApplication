
const Status = "@ViewBag.PageStatus"
document.addEventListener("load", CheckErrorPageStatus(Status));

function UpdateHdnSavingGoal() {
    var txtSavingsGoal = document.getElementById('txtSavingsGoal').value;
    var numSavingsGoal = txtSavingsGoal.replace('£', '').replace(',', '');
    document.getElementById('SavingsGoal').value = numSavingsGoal;
}

function UpdateHdnCurrentBalance() {
    var txtSavingsGoal = document.getElementById('txtCurrentBalance').value;
    var numSavingsGoal = txtSavingsGoal.replace('£', '').replace(',', '');
    document.getElementById('CurrentBalance').value = numSavingsGoal;
}

function CheckErrorPageStatus(Status) {
    if (Status == "Savings Name Error Regular") {
        LoadRegularSavingYes();
    }
    else if (Status == "Savings Name Error NotRegular") {
        LoadRegularSavingNo();
    }
}

function LoadRegularSavingNo() {
    document.getElementById('divIsRegularSavingSelector').style.display = 'none';
    document.getElementById('divSavingDetails').style.visibility = 'collapse';
    document.getElementById('divSavingsTypeSelector').style.display = 'flex';
    document.getElementById('divCategoryExp').style.display = 'none';
    document.getElementById('SavingsDetails').style.display = 'block';
    //document.getElementById('btnSubmit').style.display = 'block';
    document.getElementById('isRegularSaving').value = 'False';
    document.getElementById('rdbSavingsBuilder').Checked = true;
    document.getElementById('divSavingTarget').style.display = 'flex';
    document.getElementById('divCurrentBalance').style.display = 'flex';
    document.getElementById('divGoalDate').style.display = 'none';
    document.getElementById('divSavingValue').style.display = 'none';
    document.getElementById('divCanExceedGoal').style.display = 'none';
    document.getElementById('divIsAutoComplete').style.display = 'none';
}

function LoadRegularSavingYes() {
    document.getElementById('divIsRegularSavingSelector').style.display = 'none';
    document.getElementById('divSavingsTypeSelector').style.display = 'flex';
    document.getElementById('SavingsDetails').style.display = 'none';
    //document.getElementById('btnSubmit').style.display = 'block';
    document.getElementById('isRegularSaving').value = 'True';
}

function LoadSavingTypeTargetDate() {
    document.getElementById('SavingsType').value = 'TargetDate';
    document.getElementById('divTargetDate').style.backgroundColor = '#e95420';
    document.getElementById('divSavingsBuilder').style.backgroundColor = '#fff';
    document.getElementById('divTargetAmount').style.backgroundColor = '#fff';
    document.getElementById('divSavingTarget').style.display = 'flex';
    document.getElementById('divCurrentBalance').style.display = 'flex';
    document.getElementById('divGoalDate').style.display = 'flex';
    document.getElementById('divSavingValue').style.display = 'flex';
    document.getElementById('divCanExceedGoal').style.display = 'none';
    document.getElementById('divIsAutoComplete').style.display = 'flex';
    document.getElementById('txtRegularSavingValue').disabled = true;
    document.getElementById('GoalDate').disabled = false;
    document.getElementById('ddlSavingsPeriod').value = 'PerDay'
    document.getElementById('ddlSavingsPeriod').disabled = true;
    document.getElementById('divCategoryExp').innerHTML = '<p style="font-size: 10px" class="pb-1 ps-2 pe-2 m-0">For holidays, your wedding ...</p><p style="font-size: 10px" class="pb-1 ps-2 pe-2 m-0">Figure out how much you need to contribute each day to meet a goal with a deadline</p>'
}

function LoadSavingTypeSavingsBuilder() {
    document.getElementById('SavingsType').value = 'SavingsBuilder';
    document.getElementById('divTargetDate').style.backgroundColor = '#fff';
    document.getElementById('divSavingsBuilder').style.backgroundColor = '#e95420';
    document.getElementById('divTargetAmount').style.backgroundColor = '#fff';
    document.getElementById('divSavingTarget').style.display = 'none';
    document.getElementById('divCurrentBalance').style.display = 'flex';
    document.getElementById('divGoalDate').style.display = 'none';
    document.getElementById('divSavingValue').style.display = 'flex';
    document.getElementById('divCanExceedGoal').style.display = 'none';
    document.getElementById('divIsAutoComplete').style.display = 'none';
    document.getElementById('txtRegularSavingValue').disabled = false;
    document.getElementById('GoalDate').disabled = false;
    document.getElementById('ddlSavingsPeriod').disabled = false;
    document.getElementById('divCategoryExp').innerHTML = '<p style="font-size: 10px" class="pb-1 ps-2 pe-2 m-0">For saving goals without any time or target constraints</p><p style="font-size: 10px" class="pb-1 ps-2 pe-2 m-0">Save the same amount each period, as much as you can afford!</p>'

}

function LoadSavingTypeTargetAmount() {
    document.getElementById('SavingsType').value = 'TargetAmount';
    document.getElementById('divTargetDate').style.backgroundColor = '#fff';
    document.getElementById('divSavingsBuilder').style.backgroundColor = '#fff';
    document.getElementById('divTargetAmount').style.backgroundColor = '#e95420';
    document.getElementById('divSavingTarget').style.display = 'flex';
    document.getElementById('divCurrentBalance').style.display = 'flex';
    document.getElementById('divGoalDate').style.display = 'flex';
    document.getElementById('divSavingValue').style.display = 'flex';
    document.getElementById('divCanExceedGoal').style.display = 'flex';
    document.getElementById('divIsAutoComplete').style.display = 'flex';
    document.getElementById('txtRegularSavingValue').disabled = false;
    document.getElementById('GoalDate').disabled = true;
    document.getElementById('ddlSavingsPeriod').disabled = false;
    document.getElementById('divCategoryExp').innerHTML = '<p style="font-size: 10px" class="pb-1 ps-2 pe-2 m-0">For emergency fund, downpayment ...</p><p style="font-size: 10px" class="pb-1 ps-2 pe-2 m-0">Save towards a target over time, contribute as much as you can each period</p>'

}

const radioButtons = document.querySelectorAll('input[name="SavingTypeToggleOption"]');

radioButtons.forEach(radio => {
    radio.addEventListener('click', HandleSavingTypeRadioClick);
});

document.getElementById("txtSavingsGoal").addEventListener('change', UpdateValues, false);
document.getElementById("txtCurrentBalance").addEventListener('change', UpdateValues, false);
//document.getElementById("GoalDate").addEventListener('change', CheckDateFuture, UpdateValues, false);
document.getElementById("RegularSavingValue").addEventListener('change', UpdateValues, false);
document.getElementById('ddlSavingsPeriod').addEventListener('change', UpdateValues, false);

function isRegularSavingSelection(isRegular) {
    if (isRegular == 'Yes') {
        LoadRegularSavingYes();
    }
    else if (isRegular == 'No') {
        LoadRegularSavingNo();
    }
}

function HandleSavingTypeRadioClick() {
    document.getElementById('SavingsDetails').style.display = 'block';
    ClearSavingsInputs();
    HideValidationMessages();
    if (document.getElementById('rdbTargetDate').checked) {
        LoadSavingTypeTargetDate();
    }
    else if (document.getElementById('rdbSavingsBuilder').checked) {
        LoadSavingTypeSavingsBuilder();
    }
    else if (document.getElementById('rdbTargetAmount').checked) {
        LoadSavingTypeTargetAmount();
    }
}

function UpdateValues() {
    //console.log('UpdatingValues');
    if (document.getElementById('rdbTargetDate').checked) {
        //console.log('TargetDate');
        UpdateSavingsValue();
    }
    else if (document.getElementById('rdbTargetAmount').checked) {
        //console.log('TargetAmount');
        UpdateGoalDate();
    }
}

function UpdateSavingsValue() {
    if (document.getElementById('txtSavingsGoal').value == '' || document.getElementById('txtCurrentBalance').value == '' || document.getElementById('hdnServerDate').value == '' || document.getElementById('GoalDate').value == '') {

        document.getElementById('txtRegularSavingValue').value = '';
    }
    else {
        var SpendPerDay = CalculateSavingsValue();
        document.getElementById('txtRegularSavingValue').value = '£' + SpendPerDay;
        document.getElementById('RegularSavingValue').value = SpendPerDay;
    }

}

function UpdateGoalDate() {
    if (document.getElementById('txtSavingsGoal').value == '' || document.getElementById('txtCurrentBalance').value == '' || document.getElementById('hdnServerDate').value == '' || document.getElementById('RegularSavingValue').value == '') {

        document.getElementById('GoalDate').value = '';
        //console.log('Not all values Set');
    }
    else {
        //console.log('all values Set');
        var GoalCompleteDate = CalculateGoalDate();
        document.getElementById('GoalDate').value = GoalCompleteDate;
    }

}

function CalculateSavingsValue() {
    var SavingGoal = Number((document.getElementById('txtSavingsGoal').value).replace(/[^0-9.-]+/g, ""));
    var CurrentSavings = Number((document.getElementById('txtCurrentBalance').value).replace(/[^0-9.-]+/g, ""));
    var CurrentDateString = (document.getElementById('hdnServerDate').value);
    var CurrentDateStringSplit = CurrentDateString.split(' ');
    var DateCurrentDate = new Date(CurrentDateStringSplit[0].split('/')[2], CurrentDateStringSplit[0].split('/')[1] - 1, CurrentDateStringSplit[0].split('/')[0]);
    var CurrentDate = DateCurrentDate.getTime();
    var TargetDate = Date.parse(document.getElementById('GoalDate').value);
    //console.log(document.getElementById('GoalDate').value);

    var NumberOfDays = (TargetDate - CurrentDate) / (1000 * 3600 * 24);
    var SpendPer = (SavingGoal - CurrentSavings) / NumberOfDays;

    return SpendPer.toFixed(2);



}

function CheckDateFuture() {
    var CurrentDateString = (document.getElementById('hdnServerDate').value);
    var CurrentDateStringSplit = CurrentDateString.split(' ');
    var DateCurrentDate = new Date(CurrentDateStringSplit[0].split('/')[2], CurrentDateStringSplit[0].split('/')[1] - 1, CurrentDateStringSplit[0].split('/')[0]);
    var CurrentDate = DateCurrentDate.getTime();
    var TargetDate = Date.parse(document.getElementById('GoalDate').value);

    console.log(CurrentDateString)
    console.log(CurrentDateStringSplit)
    console.log(DateCurrentDate)
    console.log(TargetDate)
    console.log(CurrentDate)

    var DateTargetDate = new Date(TargetDate)
    var year = DateTargetDate.getFullYear()

    if (!(year <= 9)) {
        if (CurrentDate > TargetDate) {
            document.getElementById('GoalDate').value = '';
            document.getElementById('valGoalDate').innerHTML = '<span id="GoalDate-error" style="font-size:8pt" class> * The Date must be in the future</span>'
        }
        else {
            document.getElementById('valGoalDate').innerHTML = ''
        }
    }
}

function CalculateGoalDate() {
    var SavingGoal = Number((document.getElementById('txtSavingsGoal').value).replace(/[^0-9.-]+/g, ""));
    var CurrentSavings = Number((document.getElementById('txtCurrentBalance').value).replace(/[^0-9.-]+/g, ""));
    if (document.getElementById('ddlSavingsPeriod').value = 'PerDay') {
        var DailySavingsAmount = Number((document.getElementById('txtRegularSavingValue').value).replace(/[^0-9.-]+/g, ""));
    }
    else if (document.getElementById('ddlSavingsPeriod').value = 'PerPayPeriod') {
        var DailySavingsAmount = Number((document.getElementById('txtRegularSavingValue').value).replace(/[^0-9.-]+/g, "")) / @ViewBag.PaymentPeriod;
    }
    var CurrentDateString = (document.getElementById('hdnServerDate').value);
    var CurrentDateStringSplit = CurrentDateString.split(' ');
    var DateCurrentDate = new Date(CurrentDateStringSplit[0].split('/')[2], CurrentDateStringSplit[0].split('/')[1] - 1, CurrentDateStringSplit[0].split('/')[0]);
    var CurrentDate = DateCurrentDate.getTime();

    var NumberOfDays = (SavingGoal - CurrentSavings) / DailySavingsAmount;
    var NumberOfDaysTime = NumberOfDays * (1000 * 3600 * 24);
    //console.log(NumberOfDays);
    var GoalCompleteDate = new Date(CurrentDate + NumberOfDaysTime);

    var month = (GoalCompleteDate.getMonth() + 1)
    if (month < 10) {
        month = '0' + month
    }

    var day = GoalCompleteDate.getDate()
    if (day < 10) {
        day = '0' + day
    }

    var year = GoalCompleteDate.getFullYear()

    var Goal_date = year + "-" + month + "-" + day;

    return Goal_date;
}

function ClearSavingsInputs() {
    document.getElementById('txtSavingsGoal').value = '';
    document.getElementById('txtCurrentBalance').value = '';
    document.getElementById('GoalDate').value = '';
    document.getElementById('txtRegularSavingValue').value = '';
    document.getElementById('isAutoComplete').Checked = false;
    document.getElementById('canExceedGoal').Checked = false;
}

function HideValidationMessages() {
    const ValidationMessages = document.querySelectorAll('span.text-danger');
    ValidationMessages.forEach(ValMess => {
        ValMess.innerHTML = '';
    });
}

function UpdateHdnSavingAmount() {
    if (document.getElementById('ddlSavingsPeriod').value = 'PerDay') {
        var txtSavingsGoal = document.getElementById('txtRegularSavingValue').value;
        var numSavingsGoal = txtSavingsGoal.replace('£', '').replace(',', '');
        document.getElementById('RegularSavingValue').value = numSavingsGoal;
    }
    else if (document.getElementById('ddlSavingsPeriod').value = 'PerPayPeriod') {

        var txtSavingsGoal = document.getElementById('txtRegularSavingValue').value;
        var numSavingsGoal = txtSavingsGoal.replace('£', '').replace(',', '');
        var numSavingsGoal = numSavingsGoal / @ViewBag.PaymentPeriod
        document.getElementById('RegularSavingValue').value = numSavingsGoal;

    }
}
