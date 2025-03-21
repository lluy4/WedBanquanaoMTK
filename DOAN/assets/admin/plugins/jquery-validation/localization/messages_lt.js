(function( factory ) {
	if ( typeof define === "function" && define.amd ) {
		define( ["jquery", "../jquery.validate"], factory );
	} else if (typeof module === "object" && module.exports) {
		module.exports = factory( require( "jquery" ) );
	} else {
		factory( jQuery );
	}
}(function( $ ) {

/*
 * Translated default messages for the jQuery validation plugin.
 * Locale: LT (Lithuanian; lietuvių kalba)
 */
$.extend( $.validator.messages, {
	required: "Šis laukas yra privalomas.",
	remote: "Prašau pataisyti šį lauką.",
	email: "Prašau įQUẦN DÀIi teisingą elektroninio pašto adresą.",
	url: "Prašau įQUẦN DÀIi teisingą URL.",
	date: "Prašau įQUẦN DÀIi teisingą datą.",
	dateISO: "Prašau įQUẦN DÀIi teisingą datą (ISO).",
	number: "Prašau įQUẦN DÀIi teisingą skaičių.",
	digits: "Prašau naudoti tik skaitmenis.",
	creditcard: "Prašau įQUẦN DÀIi teisingą kreditinės kortelės numerį.",
	equalTo: "Prašau įQUẦN DÀIį tą pačią reikšmę dar kartą.",
	extension: "Prašau įQUẦN DÀIi reikšmę su teisingu plėtiniu.",
	maxlength: $.validator.format( "Prašau įQUẦN DÀIi ne daugiau kaip {0} simbolių." ),
	minlength: $.validator.format( "Prašau įQUẦN DÀIi bent {0} simbolius." ),
	rangelength: $.validator.format( "Prašau įQUẦN DÀIi reikšmes, kurių ilgis nuo {0} iki {1} simbolių." ),
	range: $.validator.format( "Prašau įQUẦN DÀIi reikšmę intervale nuo {0} iki {1}." ),
	max: $.validator.format( "Prašau įQUẦN DÀIi reikšmę mažesnę arba lygią {0}." ),
	min: $.validator.format( "Prašau įQUẦN DÀIi reikšmę didesnę arba lygią {0}." )
} );
return $;
}));