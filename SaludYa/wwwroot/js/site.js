// ════════════════════════════════════════════════════════════════
// CONFIRMACIÓN PERSONALIZADA (reemplaza confirm() nativo)
// Soporta 1 o varios pasos (doble confirmación) sin cerrar/reabrir
// el modal entre pasos, para evitar el problema de la animación.
// ════════════════════════════════════════════════════════════════
function confirmarSecuencia(pasos) {
    return new Promise((resolve) => {
        const modalEl = document.getElementById('modalConfirmacionGlobal');
        const tituloEl = document.getElementById('confirmTitulo');
        const mensajeEl = document.getElementById('confirmMensaje');
        const btnConfirmar = document.getElementById('confirmBtnAceptar');
        const btnCancelar = document.getElementById('confirmBtnCancelar');
        const modal = bootstrap.Modal.getOrCreateInstance(modalEl, { backdrop: 'static' });

        let indice = 0;
        let resuelto = false;

        function pintarPaso() {
            const p = pasos[indice];
            tituloEl.textContent = p.titulo || 'Confirmar acción';
            mensajeEl.innerHTML = p.mensaje || '¿Estás seguro?';
            btnConfirmar.textContent = p.textoBoton || 'Confirmar';
            btnConfirmar.className = 'btn ' + (p.claseBoton || 'btn-danger');
        }

        function limpiar() {
            btnConfirmar.removeEventListener('click', onConfirmar);
            btnCancelar.removeEventListener('click', onCancelar);
            modalEl.removeEventListener('hidden.bs.modal', onHidden);
        }

        function finalizar(valor) {
            if (resuelto) return;
            resuelto = true;
            limpiar();
            resolve(valor);
        }

        function onConfirmar() {
            indice++;
            if (indice >= pasos.length) {
                modal.hide();
                finalizar(true);
            } else {
                pintarPaso(); // siguiente paso: mismo modal, solo cambia el contenido
            }
        }

        function onCancelar() {
            modal.hide();
            finalizar(false);
        }

        function onHidden() {
            finalizar(false);
        }

        btnConfirmar.addEventListener('click', onConfirmar);
        btnCancelar.addEventListener('click', onCancelar);
        modalEl.addEventListener('hidden.bs.modal', onHidden);

        pintarPaso();
        modal.show();
    });
}

// Intercepta cualquier <form class="form-confirmar"> del sitio.
// Soporta doble confirmación con data-doble-confirmacion="true"
document.addEventListener('submit', async function (e) {
    const form = e.target;
    if (!form.classList.contains('form-confirmar')) return;
    if (form.dataset.confirmado === 'true') return; // ya confirmado, dejar pasar

    e.preventDefault();

    const pasos = [{
        titulo: form.dataset.titulo,
        mensaje: form.dataset.mensaje,
        textoBoton: form.dataset.boton,
        claseBoton: form.dataset.claseBoton
    }];

    if (form.dataset.dobleConfirmacion === 'true') {
        pasos.push({
            titulo: form.dataset.titulo2 || '¿Estás realmente seguro?',
            mensaje: form.dataset.mensaje2 || 'Esta acción afecta a todo el centro y a lo que depende de él. Confirmá nuevamente.',
            textoBoton: form.dataset.boton2 || 'Sí, desactivar definitivamente',
            claseBoton: 'btn-danger'
        });
    }

    const ok = await confirmarSecuencia(pasos);
    if (!ok) return;

    form.dataset.confirmado = 'true';
    form.submit();
});

// ════════════════════════════════════════════════════════════════
// BÚSQUEDA EN VIVO + CAJONES PLEGABLES
// ════════════════════════════════════════════════════════════════
function habilitarBusquedaConCajones(inputId, selectorGrupo, selectorItem) {
    const input = document.getElementById(inputId);
    if (!input) return;

    input.addEventListener('input', function () {
        const q = this.value.trim().toLowerCase();

        document.querySelectorAll(selectorGrupo).forEach(grupo => {
            let hayMatchEnGrupo = false;
            grupo.querySelectorAll(selectorItem).forEach(item => {
                const texto = (item.dataset.buscar || item.textContent).toLowerCase();
                const matchea = q === '' || texto.includes(q);
                item.style.display = matchea ? '' : 'none';
                if (matchea && q !== '') hayMatchEnGrupo = true;
            });

            const collapseTarget = grupo.querySelector('.collapse');
            const grupoBuscar = (grupo.dataset.buscar || '').toLowerCase();

            if (collapseTarget) {
                grupo.style.display = (q === '' || hayMatchEnGrupo || grupoBuscar.includes(q)) ? '' : 'none';

                if (q !== '' && hayMatchEnGrupo && !collapseTarget.classList.contains('show')) {
                    bootstrap.Collapse.getOrCreateInstance(collapseTarget, { toggle: false }).show();
                }
                if (q === '') {
                    bootstrap.Collapse.getOrCreateInstance(collapseTarget, { toggle: false }).hide();
                }
            } else {
                // Ítem "plano" (ej: Centros, donde cada fila es su propio grupo)
                grupo.style.display = (q === '' || grupoBuscar.includes(q)) ? '' : 'none';
            }
        });
    });
}

// Filtro simple para listas sin cajones (una sola lista de ítems)
function habilitarBusquedaSimple(inputId, selectorItem) {
    const input = document.getElementById(inputId);
    if (!input) return;
    input.addEventListener('input', function () {
        const q = this.value.trim().toLowerCase();
        document.querySelectorAll(selectorItem).forEach(item => {
            item.style.display = (item.dataset.buscar || '').toLowerCase().includes(q) ? '' : 'none';
        });
    });
}

// ════════════════════════════════════════════════════════════════
// MOSTRAR / OCULTAR DESACTIVADOS
// ════════════════════════════════════════════════════════════════
function habilitarToggleInactivos(btnId, selectorInactivos) {
    const btn = document.getElementById(btnId);
    if (!btn) return;

    let ocultos = true; // arranca ocultando los desactivados

    function aplicar() {
        document.querySelectorAll(selectorInactivos).forEach(el => {
            el.classList.toggle('oculto-por-toggle', ocultos);
        });
        btn.textContent = ocultos ? '👁 Mostrar desactivados' : '🙈 Ocultar desactivados';
        btn.classList.toggle('btn-outline-secondary', ocultos);
        btn.classList.toggle('btn-secondary', !ocultos);
    }

    btn.addEventListener('click', () => {
        ocultos = !ocultos;
        aplicar();
    });

    aplicar();
}