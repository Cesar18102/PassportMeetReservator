const BROWSER_NUMBER = 5;
const OPERATION = 9;
const WAIT = 50;

const NAME = "Ivanna";
const SURNAME = "Kolomoets";
const EMAIL = "ivanna.kolom@gmail.com";

const START_AT = new Date(2021, 0, 24, 6, 59, 55, 0);

let VUE = document.getElementsByClassName('container-fluid')[0].childNodes[0].__vue__;

function fillForm() {
	if(VUE.properties != undefined && VUE.properties.length == 3) {		
		VUE.properties[0].value = NAME;
		VUE.properties[1].value = SURNAME;
		VUE.properties[2].value = EMAIL;
		
		VUE.continueReservation();
		VUE.acceptRegulations = true;
		VUE.continueReservation();
	} else {
        setTimeout(fillForm, WAIT);
    }
}
async function selectTime() {
    await VUE.refreshSlots();
    if (VUE.availableSlots.length != 0 && VUE.availableSlots.length > BROWSER_NUMBER) {
        VUE.selectedSlot = VUE.availableSlots[BROWSER_NUMBER];
		await VUE.blockSlot();
		VUE.continueReservation();
		fillForm();
    } else {
        setTimeout(selectTime, WAIT);
    }
}
async function selectDate() {
    await VUE.getAvailableDates();
    if (VUE.minAvailableDate.toString() != 'Invalid Date') {
        VUE.selectedDay = VUE.minAvailableDate;
        await selectTime();
    } else {
        setTimeout(selectDate, WAIT);
    }
}

async function startAt(action, time) {
	if(new Date() >= time) {
		await action();
	} else {
		setTimeout(startAt, WAIT, action, time);
	}
}

startAt(async () => {
	VUE.selectedOperation = OPERATION;
	await selectDate();
}, START_AT);