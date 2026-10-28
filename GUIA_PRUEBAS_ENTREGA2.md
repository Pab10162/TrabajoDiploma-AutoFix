# Entrega 2 - RFN2 "Registrar Diagnóstico del Vehículo" - Guía de pruebas

## Antes de ejecutar
1. SSMS: correr en orden `Scripts_SQL\1_...sql`, `2_...sql`, `3_...sql`, `4_...sql` (base nueva: antes `SistemaBase\Script_Create.sql`).
2. Visual Studio: abrir `TallerMecanicoTD.sln`, Compilar solución. Proyecto de inicio: GUI_56PS.
3. Necesitás un turno de HOY (Pendiente o Confirmado): cargarlo con recepcion en Turnos > Asignar turno.

## Usuarios (Script_Create)
| Usuario | Contraseña | Rol | Menú Diagnóstico |
|---|---|---|---|
| recepcion | Recep123 | Recepcionista | Registrar asistencia, Registrar cobro |
| jefe | Jefe123 | Jefe de taller | Registrar revisión |
| mecanico | Meca123 | Mecánico | Registrar diagnóstico, Registrar reparación |
| admin | Admin123 | Administrador | todo |

## Flujo (en este orden)
1. recepcion: Diagnóstico > Registrar asistencia: elegir el turno de hoy > Registrar llegada.
2. jefe: Diagnóstico > Registrar revisión: elegir turno, mecánico "mecanico", escribir observaciones > Registrar revisión.
3. mecanico: Diagnóstico > Registrar diagnóstico: sistema afectado, urgencia, fallas > Registrar diagnóstico. Pasa solo a la pestaña 2.
4. mecanico (pestaña 2): tipo y descripción de la solución > Guardar solución. Opcional: agregar un repuesto faltante (queda PendienteCompra) y probar que Registrar reparación lo bloquea hasta "Marcar comprado".
5. mecanico: Diagnóstico > Registrar reparación: tareas y repuestos > Registrar reparación; luego presupuesto final > Finalizar reparación.
6. recepcion: Diagnóstico > Registrar cobro: monto (sugerido = presupuesto), medio de pago > Registrar cobro. El turno queda Atendido.

## Qué comprobar
- Cada paso solo aparece/habilita con su permiso (P65..P69).
- No se puede saltar un paso (ej.: revisión sin asistencia, cobro sin reparación finalizada).
- Mensajes de error traducidos (Usuario > Cambiar idioma).
- Bitácora de eventos: módulos Asistencia, Revision, Diagnostico, Repuestos, Reparacion, Cobro.
- Dígitos verificadores: las tablas nuevas se recalculan en cada alta.
