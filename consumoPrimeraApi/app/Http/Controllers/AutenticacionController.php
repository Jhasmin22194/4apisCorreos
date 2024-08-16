<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Http;
use App\Exports\UserExport;
use Maatwebsite\Excel\Facades\Excel;

class AutenticacionController extends Controller
{
    private $usuario = 'Correos';
    private $contrasena = 'AGBClp2020';

    public function autenticar()
    {
        $datos = $this->obtenerDatos();
        return view('clientes', compact('datos'));
    }

    public function filtrarPorFecha(Request $request)
    {
        $fecha = $request->input('fecha');
        $nombre = $request->input('nombre');
        $datos = $this->obtenerDatosFiltrados($fecha, $nombre);
        return view('clientes', compact('datos'));
    }

    private function obtenerDatosFiltrados($fecha, $nombre)
    {
        $response = Http::withoutVerifying()->withToken($this->obtenerToken())
            ->post('http://localhost:5254/api/O_MAIL_OBJECTS/filtrar', [
                'fecha' => $fecha,
                'nombre' => $nombre,
            ]);
        if ($response->successful()) {
            return $response->json();
        } else {
            abort(500, 'Error al obtener los datos filtrados de la API');
        }
    }
    private function obtenerDatos()
    {
        $response = Http::withoutVerifying()->withToken($this->obtenerToken())
            ->get('http://localhost:5254/api/O_MAIL_OBJECTS');
        if ($response->successful()) {
            return $response->json();
        } else {
            abort(500, 'Error al obtener los datos de la API');
        }
    }
    private function obtenerToken()
    {
        $response = Http::withoutVerifying()->post('http://localhost:5254/api/Autenticacion/Validaar', [
            'correo' => $this->usuario,
            'clave' => $this->contrasena,
        ]);
        if ($response->successful()) {
            return $response->json('token');
        } else {
            abort(500, 'Error al obtener el token de autenticación');
        }
    }
    public function export(Request $request)
{
    $fecha = $request->input('fecha');
    $nombre = $request->input('nombre');
    $datosFiltrados = $this->obtenerDatosFiltrados($fecha, $nombre);
    return Excel::download(new UserExport($datosFiltrados), 'usuarios.xlsx');
}
}
