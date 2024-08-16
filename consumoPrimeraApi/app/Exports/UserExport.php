<?php

namespace App\Exports;

use Maatwebsite\Excel\Concerns\FromCollection;
use Maatwebsite\Excel\Concerns\WithHeadings;
use Maatwebsite\Excel\Concerns\ShouldAutoSize;
use Maatwebsite\Excel\Concerns\WithStyles;
use PhpOffice\PhpSpreadsheet\Worksheet\Worksheet;

class UserExport implements FromCollection, WithHeadings, ShouldAutoSize, WithStyles
{
    protected $datosFiltrados;

    public function __construct($datosFiltrados)
    {
        $this->datosFiltrados = $datosFiltrados;
    }

    public function headings(): array
    {
        return [
            'USUARIO',
            'CÓDIGO',
            'TIPO',
            'DESTINATARIO',
            'REMITENTE',
            'PESO',
            'FECHA',
            'TOTAL',
            'DESTINO',
            'PAÍS',
            'ORIGEN',
            'PAÍS',
        ];
    }

    public function collection()
    {
        return collect($this->datosFiltrados);
    }

    public function styles(Worksheet $sheet)
    {
        return [
            1 => ['font' => ['bold' => true]],
        ];
    }
}






//