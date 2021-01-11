var VUE = document.getElementsByClassName('container-fluid')[0].childNodes[0].__vue__; 

function confirmBlock(unwatcher) { 
	unwatcher(); 
	document.getElementsByClassName('btn footer-btn btn-secondary btn-lg btn-block')[0].click(); 
} 
function block(unwatcher) { 
	unwatcher(); 
	let selector = document.getElementById('selectTime'); 
	let confirmUnwatcher = selector.__vue__.$watch( 
		function() { 
			return this.localValue; 
		}, 
		function(newValue, oldValue) { 
			confirmBlock(confirmUnwatcher); 
		} 
	); 
	selector.selectedIndex = 1; 
	let e = document.createEvent('HTMLEvents'); 
	e.initEvent('change', true, true); 
	selector.dispatchEvent(e); 
} 
function selectTime(day) { 
	let selector = document.getElementById('selectTime'); 
	let unwatcher = selector.__vue__.$watch( 
		function() { 
			return this.formOptions; 
		}, 
		function(newValue, oldValue) { 
			block(unwatcher); 
		} 
	); 
	VUE.selectedDay = '2021-01-25'; 
	document.getElementsByClassName('btn footer-btn btn-secondary btn-lg btn-block')[0].click(); 
} 
function findOperationButton(operation) { 
	let views = document.getElementsByClassName('operation-button'); 
	for(let view of views) { 
		if(view.textContent.indexOf(operation) != -1) { 
			return view; 
		 } 
	} 
	return null;
} 
var days = document.getElementsByClassName('vc-day id-2021-01-25'); 
days[0].__vue__.$watch( 
	function() { 
		return this.$props.day.isDisabled; 
	}, 
	function(newValue, oldValue) { 
		if(newValue === false && VUE.selectedOperation == 7073) 
		{ 
			selectTime(days[0]); 
		} 
	} 
); 
VUE.selectedOperation = 7073;