<?php

use Illuminate\Support\Facades\Route;
use App\Http\Controllers\AutenticacionController;

// Ruta para mostrar el formulario de filtrado por fecha
Route::get('/', function () {
    return view('formulario_filtrado');
});

// Ruta para manejar la solicitud de filtrado por fecha y nombre de usuario
Route::post('/filtrar', [AutenticacionController::class, 'filtrarPorFecha'])->name('filtrar');

// Ruta para exportar en Excel
Route::get('/exportar', [AutenticacionController::class, 'export'])->name('exportar');

// Ruta para generar y exportar los datos en PDF
Route::get('/export-pdf', [AutenticacionController::class, 'exportPDF'])->name('pdf');
