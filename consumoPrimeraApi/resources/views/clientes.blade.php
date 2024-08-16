<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Clientes</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous">
</head>

<body>
    <div class="container-fluid">
        <div class="container-fluid">
            <h1>FILTRAR POR FECHA Y USUARIO</h1>
            <form action="{{ route('filtrar') }}" method="post">
                @csrf
                <div class="row">
                    <div class="col">
                        <div class="mb-3">
                            <label for="fecha" class="form-label">Fecha:</label>
                            <input type="date" id="fecha" name="fecha" class="form-control" value="{{ old('fecha') }}" required>
                        </div>
                    </div>
                    <div class="col">
                        <div class="mb-3">
                            <label for="nombre" class="form-label">Nombre:</label>
                            <input type="text" id="nombre" name="nombre" class="form-control" value="{{ old('nombre') }}" required>
                        </div>
                    </div>
                </div>
                <button type="submit" class="btn btn-primary">Filtrar</button>
                <a href="{{ route('exportar', ['fecha' => Request::input('fecha'), 'nombre' => Request::input('nombre')]) }}" class="btn btn-success">Descargar Excel</a>

            </form>
        </div>

        <div class="container-fluid">

            <table class="table table-hover">
                <thead>
                    <tr>
                        <th>NRO</th>
                        <th>Usuario</th>
                        <th>Codigo</th>
                        <th>Tipo</th>
                        <th>Destinatario</th>
                        <th>Remitente</th>
                        <th>Peso</th>
                        <th>Fecha</th>
                        <th>Total</th>
                        <th>Destino</th>
                        <th>Pais</th>
                        <th>Origen</th>
                        <th>Pais</th>
                    </tr>
                </thead>
                <tbody>
                    @php
                    $c = 0;
                    $tot = 0;
                    @endphp
                    @foreach($datos as $cliente)
                    <tr>
                        <td>{{ ++$c }}</td>
                        <td>{{ $cliente['usuario'] }}</td>
                        <td>{{ $cliente['mailObjectId'] }}</td>
                        <td>{{ $cliente['mailClassCd'] }}</td>
                        <td>{{ $cliente['sNm'] }}</td>
                        <td>{{ $cliente['rNm'] }}</td>
                        <td>{{ $cliente['gWgt'] }}</td>
                        <td>{{ $cliente['trDat'] }}</td>
                        <td>{{ $cliente['totCPVal'] }}</td>
                        @php
                        // Eliminar la coma del valor antes de sumarlo
                        $valor = str_replace(',', '.', $cliente['totCPVal']);
                        // Verificar si el valor es numérico antes de sumarlo

                        $tot += floatval($valor);

                        @endphp
                        <td>{{ $cliente['sSta'] }}</td>
                        <td>{{ $cliente['sCtr'] }}</td>
                        <td>{{ $cliente['rAdL1'] }}</td>
                        <td>{{ $cliente['rCty'] }}</td>
                    </tr>
                    @endforeach
                    <tr>
                        <td colspan="8">
                            <h>Total</h>
                        </td>
                        <td>{{ number_format($tot, 2, ',', '.') }}</td>
                        <td colspan="4"></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</body>

</html>