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
	email: "Prašau įTRANG ĐIỂMi teisingą elektroninio pašto adresą.",
	url: "Prašau įTRANG ĐIỂMi teisingą URL.",
	date: "Prašau įTRANG ĐIỂMi teisingą datą.",
	dateISO: "Prašau įTRANG ĐIỂMi teisingą datą (ISO).",
	number: "Prašau įTRANG ĐIỂMi teisingą skaičių.",
	digits: "Prašau naudoti tik skaitmenis.",
	creditcard: "Prašau įTRANG ĐIỂMi teisingą kreditinės kortelės numerį.",
	equalTo: "Prašau įTRANG ĐIỂMį tą pačią reikšmę dar kartą.",
	extension: "Prašau įTRANG ĐIỂMi reikšmę su teisingu plėtiniu.",
	maxlength: $.validator.format( "Prašau įTRANG ĐIỂMi ne daugiau kaip {0} simbolių." ),
	minlength: $.validator.format( "Prašau įTRANG ĐIỂMi bent {0} simbolius." ),
	rangelength: $.validator.format( "Prašau įTRANG ĐIỂMi reikšmes, kurių ilgis nuo {0} iki {1} simbolių." ),
	range: $.validator.format( "Prašau įTRANG ĐIỂMi reikšmę intervale nuo {0} iki {1}." ),
	max: $.validator.format( "Prašau įTRANG ĐIỂMi reikšmę mažesnę arba lygią {0}." ),
	min: $.validator.format( "Prašau įTRANG ĐIỂMi reikšmę didesnę arba lygią {0}." )
} );
return $;
}));