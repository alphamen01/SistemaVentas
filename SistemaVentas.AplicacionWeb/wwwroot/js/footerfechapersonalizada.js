// Obtiene el elemento del año actual
var currentYearElement = document.getElementById("currentYear");

// Obtiene el año actual
var currentYear = new Date().getFullYear();

// Establece el año actual en el elemento
currentYearElement.innerHTML = "Copyright &copy; Your Website  " + currentYear;