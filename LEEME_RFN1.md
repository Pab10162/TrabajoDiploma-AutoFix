# TallerMecanicoTD — SistemaBase + RFN1 "Asignar Turno de Reparación"

Este proyecto es el **SistemaBase completo** (login, usuarios, roles/familias, bitácora,
idiomas, backup/restore, dígito verificador), sin tocar nada de lo que ya funcionaba, más lo
del **TallerMecanico** que hace falta para el RFN1, **reescrito con la arquitectura y las
convenciones del SistemaBase** (sufijo `_56PS`, `DAL_56PS` genérica, DV, bitácora,
permisos por patentes, traducción por `Tag`).

## Qué se agregó

| Capa | Archivos nuevos | Qué hacen |
|---|---|---|
| BE | `BE_Cliente_56PS`, `BE_Vehiculo_56PS`, `BE_Turno_56PS` | Entidades del RFN1. El turno nace **sin mecánico** (se asigna en el RFN2). |
| DAL | `DAL_Cliente_56PS`, `DAL_Vehiculo_56PS`, `DAL_Turno_56PS` | SQL con los helpers de `DAL_56PS`. Vehículo y turno traen el cliente con **JOIN** (antes se hacía una consulta por fila). |
| BLL | `BLL_Cliente_56PS`, `BLL_Vehiculo_56PS`, `BLL_Turno_56PS`, `BLL_Taller_Comun_56PS` | Validaciones, recálculo de DV y registro en la bitácora en cada alta/modificación. |
| GUI | `Maestros/FormClientes_56PS`, `Maestros/FormVehiculos_56PS`, `Turnos/FormTurnos_56PS` | Pantallas. Menús nuevos **Maestros** (Clientes, Vehículos) y **Turnos** (Asignar turno). |
| SQL | `SistemaBase/Script_TallerMecanico.sql` | Tablas `Cliente_56PS`, `Vehiculo_56PS`, `Turno_56PS`, patentes P60–P63, familia y rol. |

Archivos del SistemaBase que se **modificaron** (pocas líneas cada uno):
`Permisos_56PS.cs` (P60–P63), `DAL_56PS.cs` (tablas nuevas para el DV),
`DAL_BackUpRestore_56PS.cs` (el instalador corre el script del taller si falta la tabla),
`MenuPrincipal_56PS.cs/.Designer.cs` (menús), `ES/EN/pt.json` (traducciones) y los `.csproj`.

## Cómo cumple cada paso del RFN1

| Paso del RFN1 | Dónde está |
|---|---|
| 1. El cliente pide turno | `FormTurnos_56PS` (pantalla de la recepcionista) |
| 2. Verificar cliente por DNI | Grupo "1. Cliente" → `BLL_Cliente_56PS.BuscarPorDni` |
| 3. Si no existe, registrarlo | Botón "Registrar cliente" → abre `FormClientes_56PS` con el DNI cargado → `BLL_Cliente_56PS.Alta` |
| 4. Verificar vehículo por patente | Grupo "2. Vehículo" (vehículos del cliente) / `FormVehiculos_56PS` → `BuscarPorPatente` |
| 5. Si no existe, registrarlo y asociarlo al cliente | Botón "Registrar vehículo" → `FormVehiculos_56PS` con el cliente cargado → `BLL_Vehiculo_56PS.Alta` |
| 6. Consultar turnos disponibles | Grupo "3. Turno": al elegir la fecha → `BLL_Turno_56PS.ObtenerHorariosDisponibles` |
| 7–8. Informar opciones y el cliente elige | Lista de horarios libres |
| 9. Registrar el turno | "Asignar turno" → `BLL_Turno_56PS.AsignarTurno` (estado inicial **Pendiente**) |
| Extra | Agenda del día: confirmar o cancelar turnos (cancelar libera el horario) |

### Reglas de negocio (todas en la BLL, no en la pantalla)
- DNI de 7 u 8 dígitos; todos los campos del cliente obligatorios; DNI único.
- Patente normalizada (`ab 123 cd` → `AB123CD`), 6 o 7 caracteres, única; el dueño tiene que existir y estar activo.
- No se asignan turnos en fechas pasadas ni en horarios de hoy que ya pasaron.
- Un turno por horario (turnos de 1 hora, de 8 a 21: constantes `HoraPrimerTurno`/`HoraUltimoTurno` en `BLL_Turno_56PS`).
- El vehículo tiene que ser del cliente y no puede tener otro turno vigente ese día.
- Estados: Pendiente → Confirmado → (Atendido, lo marca el RFN2); Pendiente/Confirmado → Cancelado.
- Además, la base tiene un índice único filtrado (Fecha + Horario, sin cancelados) por si dos personas reservan el mismo horario al mismo tiempo.
- Clientes y vehículos no se borran: **baja lógica** (Activar/Desactivar).

## Cómo ponerlo a andar

1. Abrí `TallerMecanicoTD.sln` en Visual Studio. Los paquetes NuGet se restauran solos al compilar.
2. Base de datos (en `SistemaBase/` están `Script_Create.sql`, `Script_Alter.sql` y `Script_TallerMecanico.sql`, y se copian solos a `bin`):
   - **Automático:** borrá la base `SistemaBase` si ya existía, poné el nombre de tu instancia en `instancia.txt` y ejecutá el programa. El instalador crea todo y calcula los DV.
   - **A mano:** ejecutá `Script_Create.sql` completo en SSMS. Después entrá con `admin`: aparece la pantalla de reparación (los DV quedaron vacíos) → **Recalcular DV**. Recién ahí pueden entrar los demás usuarios.

## Usuarios de prueba

| Usuario | Contraseña | Rol | Para probar |
|---|---|---|---|
| admin | Admin123 | Administrador | Todo: usuarios, roles, familias, bitácora, backup, DV y el RFN1 |
| admin2 | Admin123 | Administrador | Igual, pero entra en inglés |
| recepcion | Recep123 | Recepcionista | RFN1 completo (clientes, vehículos, turnos) y nada de Administración |
| recepcion2 | Recep123 | Recepcionista | Primer ingreso: obliga a cambiar la contraseña |
| jefe | Jefe123 | Jefe de taller | Ve la agenda de turnos (no asigna), la bitácora y el reporte |
| jefe2 | Jefe123 | Jefe de taller | Igual, pero entra en portugués |
| mecanico | Meca123 | Mecánico | Solo ve la agenda de turnos |
| mecanico2 | Meca123 | Mecánico | Está **bloqueado**: desbloquealo con admin |
| cajero | Caja123 | Cajero | Solo perfil (cambiar contraseña/idioma); su trabajo es del RFN2 |
| cajero2 | Caja123 | Cajero | Está **inactivo**: activalo con admin |

Datos cargados: 5 clientes (uno dado de baja, DNI 29555666), 7 vehículos (uno dado de baja
y otro cuyo dueño está de baja) y 8 turnos con fechas relativas a hoy (atendidos, cancelado,
pendientes y confirmados). Un DNI libre para probar el alta: cualquiera, por ejemplo 41999888.

## Prueba rápida (camino feliz + alternativos)

1. Entrar con `recepcion`. Turnos → Asignar turno → DNI inexistente → "Buscar" → "Registrar cliente" → completar → Aplicar. Vuelve solo a Turnos con el cliente cargado.
2. "Registrar vehículo" → patente `ab 123 cd` → se guarda `AB123CD`.
3. Elegir fecha de mañana → horario → "Asignar turno". Aparece en la agenda como Pendiente.
4. Intentar el mismo horario: ya no aparece en la lista. Intentar otro horario el mismo día con el mismo vehículo → "El vehículo ya tiene un turno vigente para esa fecha."
5. Cancelar el turno → el horario vuelve a estar libre.
6. Administración → Auditoría de eventos: aparecen los módulos Clientes, Vehiculos y Turnos.
7. Modificar a mano un registro de `Turno_56PS` en SSMS y volver a loguearse → el DV detecta la inconsistencia.

## Reporte del RFN1 (A02) — Reportes → Rep. 1 (RFN1) - Turnos

Pantalla de **solo consulta** (permiso P64, familia "Reportes": admin y jefe de taller).

- **Filtros:** fecha desde/hasta (por defecto el mes actual, máximo un año) y estado.
- **Listado:** N° turno, fecha, horario, estado, DNI y nombre del cliente, teléfono, patente, marca, modelo y mecánico.
- **Resumen / indicadores** (sobre todos los turnos del período):
  - cantidad por estado,
  - ocupación de la agenda = turnos no cancelados / (días × horarios por día),
  - porcentaje de cancelaciones,
  - día de la semana y horario con más turnos.
- **Exportar PDF** (A4 apaisado, mismo iTextSharp que la bitácora), con título, período, tabla y resumen.
- Generar y exportar quedan registrados en la bitácora (módulo "Reportes").

Capas: `FormReporteTurnos_56PS` → `BLL_ReporteTurnos_56PS.Generar` (valida el período y calcula
los indicadores) → `DAL_Turno_56PS.ObtenerParaReporte` (un solo SELECT con JOIN a cliente,
vehículo y usuario mecánico) → `BE_ReporteTurnos_56PS` / `BE_FilaReporteTurno_56PS`.

## Qué NO se pasó del TallerMecanico (y por qué)
- Login, usuarios, bitácora, backup/restore, cambio de contraseña e idiomas del TallerMecanico: ya los resuelve el SistemaBase (versión más completa).
- Todo lo del RFN2 (revisión, diagnóstico, solución, repuestos, tareas, subclases de usuario): queda para la 2ª entrega. `Turno_56PS.DNI_Mecanico` ya está preparado (nullable).
