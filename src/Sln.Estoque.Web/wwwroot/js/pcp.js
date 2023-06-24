// Registra novo pedido
$("#register-create-btn").click(function () {
    Swal.fire({
        title: "<h3 style='color:white'>" + 'Baixa de Pedido' + "</h3>",
        html:
            '<div class="d-flex flex-column justify-content-center align-middle">' +
            '<div class="d-flex justify-content-between align-items-center align-middle">' +
            '<label class="text-white">N° Pedido</label>' +
            '<input id="orderId" maxlength="6" oninput="onlyDigits(event)" class="form-control w-50 text-dark m-1" placeholder="N° pedido">' +
            '</div>' +
            '<div class="d-flex justify-content-between align-items-center align-middle">' +
            '<label class="text-white">Código do Layout</label>' +
            '<input id="layoutCode" maxlength="4" oninput="onlyDigits(event)" class="form-control w-50 text-dark m-1" placeholder="Código de layout">' +
            '</div>' +
            '<div class="d-flex justify-content-between align-items-center align-middle">' +
            '<label class="text-white">Quantidade</label>' +
            '<input id="quantity" maxlength="6" oninput="onlyDigits(event)" class="form-control w-50 text-dark m-1" placeholder="Quantidade">' +
            '</div>' +
            '</div>',
        color: '#fffff',
        background: '#161a1d',
        width: 500,
        confirmButtonText: 'Finalizar pedido',
        confirmButtonColor: '#198754',
        focusConfirm: false,
        preConfirm: () => {
            var orderId = Swal.getPopup().querySelector('#orderId').value;
            var layoutCode = Swal.getPopup().querySelector('#layoutCode').value;
            var quantity = Swal.getPopup().querySelector('#quantity').value;

            return { orderId: orderId, layoutCode: layoutCode, quantity: quantity };
        }
    }).then((result) => {
        if (result.isConfirmed) {
            var url = window.location.origin;
            $.ajax({
                type: 'POST',
                url: `${url}/Pcp/Register`,
                data: { orderId: result.value.orderId, layoutCode: result.value.layoutCode, quantity: result.value.quantity },
                success: function (response) {
                    if (response.code === '200') {
                        Swal.fire({
                            icon: 'success',
                            title: "<h5 style='color:white'>" + `O pedido foi finalizado!` + "</h5>",
                            showCancelButton: false,
                            showConfirmButton: true,
                            width: 600,
                            padding: '3em',
                            background: '#161a1d'
                        })
                        setTimeout(() => { window.location.reload(); }, 2000);
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: "<h3 style='color:white'>" + `Não foi possível finalizar o pedido!` + "</h3>",
                            html: "<h5 style='color:white'>" + `Revise as informações fornecidas` + "</h5>",
                            showCancelButton: false,
                            showConfirmButton: false,
                            width: 600,
                            padding: '3em',
                            background: '#161a1d'
                        })
                    }
                }
            });
        }
    });
});

// Busca de pedidos
$("#search-order-btn").on('click', function () {
    Swal.fire({
        title: "<h5 style='color:white'>" + 'Digite o número do pedido:' + "</h5>",
        html:
            '<div class="d-flex justify-content-around">' +
            '<div class="d-flex flex-column" style="width: 250px;">' +
            '<input id="input-search-order-swal" class="form-control text-dark" type="text" oninput="onlyDigits(event)" maxlength="6" id="" placeholder="Ex.: 45000">' +
            '</div>' +
            '</div>',
        showCancelButton: false,
        confirmButtonText: 'Buscar',
        confirmButtonColor: '#198754',
        width: 400,
        padding: '3em',
        color: '#fffff',
        background: '#161a1d',
        preConfirm: () => {
            const orderNumber = document.getElementById('input-search-order-swal').value;
            let url = window.location.origin;
            $.ajax({
                url: `${url}/Pcp/Search`,
                type: 'POST',
                data: {
                    ordernumber: orderNumber
                },
                success: function (resp) {
                    if (resp != null) {
                        if (resp[0].id > 0) {
                            var trHtml = '';
                            resp.forEach(function (obj) {
                                var formattedDate = moment(obj.dateFinish).format('DD/MM/YY HH:mm');
                                trHtml +=
                                    '<tr>' +
                                    '<td>' + obj.orderId + '</td>' +
                                    '<td>' + obj.layoutCode + '</td>' +
                                    '<td>' + obj.quantity + '</td>' +
                                    '<td>' + formattedDate + '</td>' +
                                    '<td>' + obj.user.name + '</td>' +
                                    '</tr>';
                            });
                            Swal.fire({
                                html:
                                    '<table id="table-swal-searchorder">' +
                                    '<thead id="head-swal-searchorder">' +
                                    '<tr>' +
                                    '<th>Pedido</th>' +
                                    '<th>Código Layout</th>' +
                                    '<th>Quantidade</th>' +
                                    '<th>Data da Finalização</th>' +
                                    '<th>Operador</th>' +
                                    '</tr>' +
                                    '</thead>' +
                                    '<tbody id="body-swal-searchorder">' +
                                    trHtml +
                                    '</tbody>' +
                                    '</table> '
                                ,
                                showCancelButton: false,
                                confirmButtonText: 'Fechar',
                                width: 1000,
                                padding: '3em',
                                color: '#fffff',
                                background: '#161a1d'
                            });
                        } else {
                            Swal.fire({
                                icon: 'error',
                                title: "<h5 style='color:white'>" + 'Pedido não encontrado.' + "</h5>",
                                showCancelButton: false,
                                width: 600,
                                padding: '3em',
                                color: '#fffff',
                                background: '#161a1d'
                            });
                        }
                    }
                    else {
                        Swal.fire({
                            icon: 'error',
                            title: "<h5 style='color:white'>" + 'Pedido não encontrado.' + "</h5>",
                            showCancelButton: false,
                            width: 600,
                            padding: '3em',
                            color: '#fffff',
                            background: '#161a1d'
                        });
                    }
                },
                error: function () {
                    Swal.fire({
                        icon: 'error',
                        title:
                            "<h5 style='color:white'>" +
                            'Houve um problema na sua requisição. Por favor, entre em contato com o suporte.' +
                            "</h5>",
                        showCancelButton: false,
                        width: 600,
                        padding: '3em',
                        color: '#fffff',
                        background: '#161a1d'
                    });
                }
            });
        }
    });
});

// Exportar CSV
$("#export-csv-pcp-btn").on('click', function () {
    Swal.fire({
        title: 'Selecione data inicial e final',
        width: 800,
        color: '#fff',
        background: '#161a1d',
        html:
            '<div class="d-flex justify-content-around" style="height: 290px;">' +
            '<div class="d-flex flex-column" style="width: 250px;">' +
            '<label class="mb-2" for="end-datepicker">Data inicial:</label>' +
            '<input class="text-center input-readonly bg-black text-white" type="text" id="start-datepicker" readonly>' +
            '</div>' +
            '<div class="d-flex flex-column" style="width: 250px;">' +
            '<label class="mb-2" for="end-datepicker">Data final:</label>' +
            '<input class="text-center input-readonly bg-black text-white" type="text" id="end-datepicker" readonly>' +
            '</div>' +
            '</div>',
        confirmButtonText: 'Baixar CSV',
        confirmButtonColor: '#198754',
        showCancelButton: true,
        cancelButtonText: 'Cancelar',
        didOpen: () => {
            const startDatepickerElement = document.getElementById('start-datepicker');
            datepicker(startDatepickerElement, customSettings);

            const endDatepickerElement = document.getElementById('end-datepicker');
            datepicker(endDatepickerElement, customSettings);
        },
        preConfirm: () => {
            const startDate = document.getElementById('start-datepicker').value;
            const endDate = document.getElementById('end-datepicker').value;
            var url = window.location.origin;
            $.ajax({
                url: `${url}/Pcp/ExportCSV`,
                type: 'GET',
                responseType: 'blob',
                data: {
                    startDate: startDate,
                    endDate: endDate
                },
                success: function (data) {
                    if (data.error) {
                        Swal.fire({
                            icon: 'error',
                            title: "<h5 style='color:white'>" + data.error + "</h5>",
                            showCancelButton: false,
                            width: 600,
                            padding: '3em',
                            color: '#fffff',
                            background: '#161a1d'
                        });
                    } else {
                        var blob = new Blob([data], { type: 'text/plain' });
                        var fileName = 'Baixas-de-Pedidos-' + getCurrentDateFormatted() + '.txt';
                        saveAs(blob, fileName);
                        Swal.fire({
                            icon: 'success',
                            title: "<h5 style='color:white'>" + 'Informações exportadas com sucesso!' + "</h5>",
                            showCancelButton: false,
                            width: 600,
                            padding: '3em',
                            color: '#fffff',
                            background: '#161a1d'
                        });
                    }
                },
                error: function (xhr, textStatus, errorThrown) {
                    if (xhr.status === 403) {
                        Swal.fire({
                            icon: 'error',
                            title: "<h5 style='color:white'>" + 'Você não tem permissão para acessar esta ação.' + "</h5>",
                            showCancelButton: false,
                            width: 600,
                            padding: '3em',
                            color: '#fffff',
                            background: '#161a1d'
                        });
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: "<h5 style='color:white'>" + 'Ocorreu um erro no seu download. Por favor, informe ao suporte.' + "</h5>",
                            showCancelButton: false,
                            width: 600,
                            padding: '3em',
                            color: '#fffff',
                            background: '#161a1d'
                        });
                    }
                }
            });
        }
    });
});

// Configurações do calendário de Export
const customSettings = {
    customDays: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S'],
    customMonths: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
    disableYearOverlay: true,
    showAllDates: true,
    dateSelected: new Date(),
    maxDate: new Date(),
    alwaysShow: true,
    formatter: (input, date, instance) => {
        const formattedDate = date.toLocaleDateString('pt-BR');
        input.value = formattedDate;
        return formattedDate;
    }
};

// Formatação de data para Export
function getCurrentDateFormatted() {
    let currentDate = new Date();
    let day = currentDate.getDate().toString().padStart(2, '0');
    let month = (currentDate.getMonth() + 1).toString().padStart(2, '0');
    let formattedDate = day + '-' + month;
    return formattedDate;
}

// Editar Pedido
const editOrder = (event, id) => {
    let button = event.target;
    let tableRow = button.closest('tr');
    let orderId = tableRow.querySelector('.order-id').textContent.replace(/\D/g, '');
    let layoutCode = tableRow.querySelector('.order-layout-code').textContent.trim();
    let quantity = tableRow.querySelector('.order-quantity').textContent.trim();

    Swal.fire({
        title: '<h3 class="text-white">' + 'Editar Pedido' + "</h3>",
        html:
            '<div class="flex-column p-4">' +
            '<div class="d-flex justify-content-between align-items-center p-3">' +
            '<label class="ms-2 text-white" for="input-order-edit-pcp">Pedido:</label>' +
            `<input class="me-2 form-control w-50" type="text" maxlength="6" oninput="onlyDigits(event)" id="input-order-edit-pcp" placeholder="Ex.: 46000" value='${orderId}'>` +
            '</div>' +
            '<div class="d-flex justify-content-between align-items-center p-3">' +
            '<label class="ms-2 text-white" for="input-layout-edit-pcp">Código:</label>' +
            `<input class="me-2 form-control w-50" type="text" maxlength="4" oninput="onlyDigits(event)" id="input-layout-edit-pcp" placeholder="Ex.: 0500" value='${layoutCode}'>` +
            '</div>' +
            '<div class="d-flex justify-content-between align-items-center p-3">' +
            '<label class="ms-2 text-white" for="input-quantity-edit-pcp">Quantidade:</label>' +
            `<input class="me-2 form-control w-50" type="text" maxlength="6" oninput="onlyDigits(event)" id="input-quantity-edit-pcp" placeholder="Ex.: 1500" value='${quantity}'>` +
            '</div>' +
            '</div>',
        color: '#fffff',
        background: '#161a1d',
        showCancelButton: true,
        confirmButtonText: 'Enviar',
        confirmButtonColor: '#198754',
        cancelButtonText: 'Cancelar',
        preConfirm: function () {
            return {
                orderId: $('#input-order-edit-pcp').val(),
                layoutCode: $('#input-layout-edit-pcp').val(),
                quantity: $('#input-quantity-edit-pcp').val(),
                id: id
            }
        },
        allowOutsideClick: false
    }).then(function (resultado) {
        if (resultado.isConfirmed) {
            let url = window.location.origin;
            $.ajax({
                url: `${url}/Pcp/Edit`,
                type: 'POST',
                dataType: 'json',
                data: resultado.value,
                success: (resp) => {
                    if (resp.code === '200') {
                        Swal.fire({
                            icon: 'success',
                            title: "<h5 style='color:white'>" + `O pedido foi editado com sucesso!` + "</h5>",
                            showCancelButton: false,
                            showConfirmButton: false,
                            width: 600,
                            padding: '3em',
                            background: '#161a1d'
                        })
                        let infos = resp.info.split('&');
                        let tdOrder = tableRow.querySelector('.order-id');
                        let tdLayoutCodeOrder = tableRow.querySelector('.order-layout-code');
                        let tdQuantity = tableRow.querySelector('.order-quantity');
                        tdOrder.textContent = `Nº ${infos[0]}`;
                        tdLayoutCodeOrder.textContent = `${infos[1]}`;
                        tdQuantity.textContent = `${infos[2]}`;
                    }
                },
                error: (erro) => {
                    Swal.fire('Erro!', erro.responseText, 'error');
                }
            });
        }
    });
};

// Deletar Pedido
const deleteOrder = (id, orderNumber) => {

    Swal.fire({
        title: "<h3 style='color:white'>" + 'Excluir Pedido' + "</h3>",
        html:
            `<p class="text-white">Tem certeza que deseja excluir esse pedido?</p>`,
        color: '#fffff',
        background: '#161a1d',
        width: 500,
        confirmButtonText: 'Sim',
        confirmButtonColor: '#dc3545',
        showCancelButton: true,
        cancelButtonText: 'Cancelar',
        focusConfirm: false,
        preConfirm: () => {
            let url = window.location.origin;
            $.ajax({
                url: `${url}/Pcp/Delete`,
                method: 'POST',
                data: {
                    id: id
                },
                success: (resp) => {
                    if (resp.code == '200') {
                        Swal.fire({
                            icon: 'success',
                            title: "<h5 style='color:white'>" + `O pedido N° ${orderNumber} foi excluído!` + "</h5>",
                            showCancelButton: false,
                            showConfirmButton: false,
                            width: 600,
                            padding: '3em',
                            background: '#161a1d'
                        })
                        setTimeout(() => { window.location.reload(); }, 3000);
                    }
                }
            });
        }
    });
};

// Disparador da searchbar do Pcp
$("#button-bar-pcp").click(function () {
    $("#input-pcp").toggle().focus();
});

// Pesquisa na tabela do Pcp
$("#input-pcp").on("keyup", function () {
    var value = $(this).val();
    $("table tr").each(function (index) {
        if (index != 0) {
            $row = $(this);
            var order = $row.find("td:nth-child(1)").text().toLowerCase().replace(/(\r\n|\n|\r|\t)/gm, "");
            var layout = $row.find("td:nth-child(2)").text().toLowerCase().replace(/(\r\n|\n|\r|\t)/gm, "");
            var quantity = $row.find("td:nth-child(3)").text().toLowerCase().replace(/(\r\n|\n|\r|\t)/gm, "");
            var user = $row.find("td:nth-child(5)").text().toLowerCase().replace(/(\r\n|\n|\r|\t)/gm, "");
            if ((order.indexOf(value.toLowerCase()) >= 0) ||
                (layout.indexOf(value.toLowerCase()) >= 0) ||
                (quantity.indexOf(value.toLowerCase()) >= 0) ||
                (user.indexOf(value.toLowerCase()) >= 0)) {
                $(this).show();
            }
            else {
                $(this).hide();
            }
        }
    });
    paintOddRows();
});
