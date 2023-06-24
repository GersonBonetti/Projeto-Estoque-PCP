$(document).ready(() => {
    $("#myInput").on("keyup", function () {
        var value = $(this).val();
        $("table tr").each(function (index) {
            if (index != 0) {
                $row = $(this);
                var name = $row.find("td:nth-child(3)").text().toLowerCase().replace(/(\r\n|\n|\r|\t)/gm, "");
                var id = $row.find("td:nth-child(2)").text().toLowerCase().replace(/(\r\n|\n|\r|\t)/gm, "");
                if ((name.indexOf(value.toLowerCase()) >= 0) ||
                    (id.indexOf(value.toLowerCase()) >= 0)) {
                    $(this).show();
                }
                else {
                    $(this).hide();
                }
            }
        });
        paintOddRows();
    });

    $(".buttonBar").click(function () {
        $("#myInput").toggle().focus();
    });

    $("#loginButton").on("click", function (e) {
        e.preventDefault();
        login();
    });

    paintOddRows();
    userCreateSuccess();
    userEditSuccess();
    nameNavbar();

    if (window.location.hash == '#unauthorized') {
        denyAccess();
    };

    $("th.sortable").click(sortTable);

    $("#export-csv-btn").on('click', function () {
        var url = window.location.origin;
        $.ajax({
            url: `${url}/Product/ExportCSV`,
            type: 'POST',
            success: function (data) {
                if (data.error) {
                    Swal.fire({
                        icon: 'error',
                        title: "<h5 style='color:white'>" + 'Houve um erro no seu download. Por favor, entre em contato com o suporte.' + "</h5>",
                        showCancelButton: false,
                        width: 600,
                        padding: '3em',
                        color: '#fffff',
                        background: '#161a1d'
                    })
                } else {
                    var blob = new Blob([data], { type: 'text/plain' });
                    var fileName = 'Estoque-' + getCurrentDateFormatted() + '.txt';
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
            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: "<h5 style='color:white'>" + 'Houve um erro no seu download. Por favor, entre em contato com o suporte.' + "</h5>",
                    showCancelButton: false,
                    width: 600,
                    padding: '3em',
                    color: '#fffff',
                    background: '#161a1d'
                })
            }
        });
    });

    /*$('#input-modal-qty').on('keyup', function () {
        let value = $(this).val();
        console.log('function está funcionando');
        value = value.replace(/[^0-9,]/g, '');
        if (value.indexOf(',') !== -1) {
            let parts = value.split(',');
            let decimalPart = parts[1];

            if (decimalPart && decimalPart.length > 2) {
                decimalPart = decimalPart.slice(0, 2);
            }
            value = parts[0] + (decimalPart ? ',' + decimalPart : '');
        }
        $(this).val(value);
    });*/
});

const onlyDigits = (event) => {
    let input = event.target;
    input.value = input.value.replace(/\D/g, '');
};

login = () => {
    let userForm = document.getElementById('username').value;
    let passwordForm = document.getElementById('password').value;
    var url = window.location.origin;
    let authAlert = setTimeout(() => {
        Swal.fire({
            title: "<h4 style='color:white'>" + 'Autenticando...' + "</h4>",
            showCancelButton: false,
            showConfirmButton: false,
            width: 450,
            padding: '3em',
            color: '#fffff',
            background: '#161a1d',
            allowOutsideClick: false,
            didOpen: () => {
                Swal.showLoading();
                Swal.getConfirmButton().blur();
            }
        });
    }, 1000);
    $.ajax({
        url: `${url}/User/SignIn`,
        method: 'POST',
        data: {
            userName: userForm,
            passWord: passwordForm
        },
        success: (resp) => {
            clearTimeout(authAlert); // Cancela o alert quando o login é bem-sucedido
            if (resp.status == 'success') {
                var user = {
                    userName: `${resp.username}`,
                    roleId: `${resp.roleId}`,
                    name: `${resp.name}`
                };
                sessionStorage.setItem('user', JSON.stringify(user));
                Cookies.set('user', `${resp.username}&${resp.roleId}&${resp.name}`, { expires: 1 / 6 });
                window.location.href = `${url}/Home/Index`;
            }
            else {
                Swal.fire({
                    icon: 'error',
                    title: "<h5 style='color:white'>" + 'Ops, algo deu errado!' + "</h5>",
                    html: "<h5 style='color:white'>" + 'Usuário e/ou senha inválidos.' + "</h5>",
                    showCancelButton: false,
                    showConfirmButton: false,
                    width: 600,
                    padding: '3em',
                    color: '#fffff',
                    background: '#161a1d'
                })
                setTimeout(() => {
                    Swal.close();
                    document.getElementById('password').value = ''; // Limpa o campo senha
                    document.getElementById('password').focus(); // Define o foco no campo senha
                }, 2000);
            }
        },
        error: () => {
            clearTimeout(authAlert); // Cancela o alert quando ocorre um erro
            Swal.fire({
                icon: 'warning',
                title: "<h5 style='color:white'>" + 'Ops, algo deu errado!' + "</h5>",
                html: "<h5 style='color:white'>" + 'Não foi possível estabelecer conexão com o banco de dados.' + "</h5>",
                showCancelButton: false,
                showConfirmButton: false,
                width: 600,
                padding: '3em',
                color: '#fffff',
                background: '#161a1d'
            })
            setTimeout(() => {
                window.location.reload()
            }, 4000);
        }
    })
};

paintOddRows = () => {
    $('tbody tr:visible:even').css({
        backgroundColor: '#d3d3d3',
        color: '#212529'
    });
    $('tbody tr:visible:odd').css({
        backgroundColor: '#fff',
        color: '#212529'
    });
}

highlightRows = () => {
    $("tr:odd").addClass('oddRow');
};

liveToastMessage = (message, origin) => {
    $('#toast-body').html(message);
    $('#toast-origin').html(origin);
    $('#toast-time').html(new Date().toLocaleTimeString('pt-BR', {
        hour12: false,
        hour: "numeric",
        minute: "numeric"
    }));
    const toastLiveMessages = $('#liveToast');
    const toast = new bootstrap.Toast(toastLiveMessages)

    toast.show()
}

modalErrorLogin = () => {
    const modalzito = $('#myModal');
    const modal = new bootstrap.Modal(modalzito);
    modal.show();
}

msgModalDel = (name, callback) => {
    let contentBody = `Deseja realmente excluir o registro "${name}"?`;

    let content = '<button id="btnModalCallback" type="button" class="btn btn-danger">Excluir</button>';

    $('#modal-body').html(contentBody);
    $('#footer-modal').html(content);
    $('#btnModalCallback').click(() => callback());

    $('#msgModal').modal('show');
}


msgModalPassword = () => {
    let contentBody = '<span class="p-2 text-white">Insira a sua nova senha:</span> <br/> <input id = "input-modal-password-user-edit1" type="password" class="mt-3" type = "text" placeholder = "Sua nova senha"/> <br/> <input id="input-modal-password-user-edit2" type="password" onkeyup="editPassword()" class="mt-3" placeholder="Repita a senha"/> <br/> <span id="span-error-password" class="p-2 text-danger disappear">As senhas estão diferentes!</span>';

    let contentFooter =
        `<a href="#" id="btn-modal-save-password" data-bs-dismiss="false" type="button" class="btn btn-success disabled" onclick="savePassword()">Salvar</a>`;

    $('#modal-body').html(contentBody);
    $('#footer-modal').html(contentFooter);

    $('#msgModal').modal('show');
}

//Salvar Senha User
savePassword = () => {
    var name = $("#input-name-user-edit").val();
    var passwordInput = $("#input-modal-password-user-edit2").val();
    console.log(username);
    console.log(passwordInput);
    var url = window.location.origin;
    $.ajax({
        url: `${url}/User/EditPassword`,
        method: 'POST',
        data: {
            name: name,
            password: passwordInput
        },
        success: (resp) => {
            if (resp.code == '200') {
                Swal.fire({
                    icon: 'success',
                    title: "<h5 style='color:white'>" + "Senha alterada com sucesso!" + "</h5>",
                    showCancelButton: false,
                    showConfirmButton: false,
                    width: 600,
                    padding: '3em',
                    background: '#161a1d'
                })
                setTimeout(() => { window.location.reload(); }, 3000);
            } else {
                Swal.fire({
                    icon: 'error',
                    title: "<h5 style='color:white'>" + "Falha ao atualizar a senha!" + "</h5>",
                    showCancelButton: false,
                    showConfirmButton: false,
                    width: 600,
                    padding: '3em',
                    background: '#161a1d'
                })
            }
        }
    });

};

//Edita Senha User
editPassword = () => {
    let input1 = $('#input-modal-password-user-edit1').val();
    let input2 = $('#input-modal-password-user-edit2').val();
    if (input1 !== input2) {
        $("#span-error-password").removeClass("disappear");
        $("#btn-modal-save-password").addClass("disabled");
    } else {
        $("#span-error-password").addClass("disappear");
        $("#btn-modal-save-password").removeClass("disabled");
    }
};

//Cria nova senha User
createPassword = () => {
    let input1 = $('#input-create-user1').val();
    let input2 = $('#input-create-user2').val();
    if (input1 !== input2) {
        $("#error-create-password-user").removeClass("disappear");
        $("#btn-create-user").addClass("disabled");
    } else {
        $("#error-create-password-user").addClass("disappear");
        $("#btn-create-user").removeClass("disabled");
    }
};

closeMsgModalMessage = () => {
    $('#msgModal').modal('hide');
}

addValue = (idParam) => {
    var qtyInput = parseFloat($('#input-modal-qty').val());
    updtQtyGeneral(idParam, qtyInput, "add");
}

minusValue = (idParam) => {
    var qtyInput = parseFloat($('#input-modal-qty').val());
    updtQtyGeneral(idParam, qtyInput, "subtract");
}

updtQtyGeneral = (idParam, qtyInput, operation) => {
    var url = window.location.origin;
    console.log(qtyInput);
    $.ajax({
        url: `${url}/Product/UpdateQuantity`,
        method: 'POST',
        data: JSON.stringify({
            Id: idParam,
            QtyInput: qtyInput,
            Operation: operation
        }),
        contentType: 'application/json',
        success: (resp) => {
            if (resp.code == '200') {
                let table = document.getElementById('tabelaIndex');
                let findRow = false;
                let i = 0;
                while (!findRow && i < table.rows.length) {
                    let row = table.rows[i];
                    let dataId = row.getAttribute("data-id");

                    if (dataId == idParam) {
                        let tdQuantity = row.querySelector(".qty-product");
                        let tdUpdateTime = row.querySelector(".updt-time-product");
                        tdQuantity.textContent = `${resp.info}`;
                        tdUpdateTime.textContent = `${resp.time}`;
                        findRow = true;
                    } else {
                        i++;
                    }
                }
            } else
                if (resp.code == '410') {
                    Swal.fire({
                        icon: 'info',
                        title: "<h5 style='color:white'>" + 'Quantidade inválida!' + "</h5>",
                        html: "<h6 style='color: #E5E5E5'>" + 'A quantidade de subtração é maior do que o que há em estoque. Por favor, atualize a página e revise a quantidade a ser retirada.' + "</h6>",
                        showCancelButton: false,
                        width: 600,
                        padding: '3em',
                        color: '#fffff',
                        background: '#161a1d'
                    })
                }
        }
    });
}

userCreateSuccess = () => {
    if (window.location.hash == '#crsuccess') {
        Swal.fire({
            icon: 'success',
            title: "<h5 style='color:white'>" + 'Usuário criado e inserido em nosso banco de dados.' + "</h5>",
            showCancelButton: false,
            width: 600,
            padding: '3em',
            color: '#fffff',
            background: '#161a1d'
        })
    }
};

userEditSuccess = () => {
    if (window.location.hash == '#edsuccess') {
        Swal.fire({
            icon: 'success',
            title: "<h5 style='color:white'>" + 'Usuário editado com sucesso!' + "</h5>",
            showCancelButton: false,
            width: 600,
            padding: '3em',
            color: '#fffff',
            background: '#161a1d'
        })
    }
};

denyAccess = () => {
    Swal.fire({
        icon: 'warning',
        title: "<h5 style='color:white'>" + 'Seu login expirou ou você não possui permissão para executar esta ação!' + "</h5>",
        showCancelButton: false,
        width: 600,
        padding: '3em',
        color: '#fffff',
        background: '#161a1d'
    })
};

logout = () => {
    sessionStorage.clear();
    Cookies.remove('user');
    Cookies.remove('userJwt');
}

$("#btnUserEdit").on("click", function (e) {
    e.preventDefault();
    listSuccess();
});

listSuccess = () => {
    Swal.fire({
        icon: 'success',
        title: "<h5 style='color:white'>" + 'A edição foi salva em nosso banco de dados' + "</h5>",
        didOpen: () => {
            Swal.showLoading()
            const b = Swal.getHtmlContainer().querySelector('b')
        },
        showCancelButton: false,
        showConfirmButton: false,
        width: 600,
        padding: '3em',
        color: '#fffff',
        background: '#161a1d'
    })
    setTimeout(() => {
        var url = window.location.origin;
        window.location.href = `${url}/User/List`;
    }, 3500)
}

nameNavbar = () => {
    var name = Cookies.get('user');
    let nameArray = name.split('&');
    let contentBody = `<span>Olá, ${nameArray[2]}</span>`;
    $('#navUsername').html(contentBody);
}

clearTable = () => {
    $("#totalFilesDiv").html('<h5 id="totalFiles">Total de Arquivos: <span id="countPartialFiles">0</span> / <span id="countToDoFiles">0</span> / <span id="countTotalFiles">0</span></h5>');
    $("#totalLabelsDiv").html('<h5 id="totalLabels">Total de Etiquetas: <span id="countPartialLabels">0</span> / <span id="countToDoLabels">0</span> / <span id="countTotalLabels">0</span></h5>');
    $("#fileTable tbody").empty();
    $("#dropzone").show();
};

clickAllRows = () => {
    var checkSelectAll = document.getElementById('inputSelectAllRows');
    var myTable = document.getElementById('fileTable');
    var checkboxes = myTable.querySelectorAll('input[type="checkbox"]');

    checkboxes.forEach((checkbox) => {
        checkbox.checked = checkSelectAll.checked;
        toggleStrike(checkbox);
    });

    var btn = document.querySelector('label[for="inputSelectAllRows"]');
    if (checkSelectAll.checked === true) {
        btn.classList.add('btn-primary');
        btn.classList.remove('btn-white');
        btn.classList.remove('border-dark');
        btn.innerHTML = 'Desmarcar todas as linhas';
    } else {
        btn.classList.remove('btn-primary');
        btn.classList.add('btn-white');
        btn.classList.add('border-dark');
        btn.innerHTML = 'Marcar todas as linhas';
    }

};

toggleStrike = (checkbox) => {
    var row = checkbox.parentNode.parentNode;
    if (checkbox.checked) {
        row.classList.add('strikethrough');
    } else {
        row.classList.remove('strikethrough');
    }
    changeTotalFiles();
    changeTotalLabels();
};

changeTotalFiles = () => {
    var myTable = document.getElementById('fileTable');
    var totalRows = myTable.querySelectorAll('tbody > tr').length;
    var checkedRows = myTable.querySelectorAll('tbody > tr.strikethrough').length;

    var partial = document.getElementById('countPartialFiles');
    partial.innerHTML = `${checkedRows}`;
    var total = document.getElementById('countTotalFiles');
    total.innerHTML = `${totalRows}`;

    var doneQuantity = parseInt(document.getElementById('countPartialFiles').textContent);
    var toDoElement = document.getElementById('countToDoFiles');
    toDoElement.innerHTML = totalRows - doneQuantity;
};

changeTotalLabels = () => {
    var myTable = document.getElementById('fileTable');
    var rows = myTable.querySelectorAll('tbody > tr');
    var totalUncheckedQuantity = 0;
    var totalLabels = 0;

    rows.forEach((row) => {
        var checkbox = row.querySelector('input[type="checkbox"]');
        var quantityCell = row.querySelector('td:nth-of-type(4)');
        var quantity = parseInt(quantityCell.textContent);
        totalLabels += quantity;

        if (checkbox.checked === true) {
            totalUncheckedQuantity += quantity;
        }
    });

    var partial = document.getElementById('countPartialLabels');
    partial.innerHTML = `${totalUncheckedQuantity}`;
    var total = document.getElementById('countTotalLabels');
    total.innerHTML = `${totalLabels}`;

    var doneQuantity = parseInt(document.getElementById('countPartialLabels').textContent);
    var toDoElement = document.getElementById('countToDoLabels');
    toDoElement.innerHTML = totalLabels - doneQuantity;
};

document.addEventListener('dragover', function (event) {
    event.preventDefault();
});

let dropzone = document.getElementById('containerTable');
dropzone.addEventListener('drop', function (event) {
    event.preventDefault();

    let files = event.dataTransfer.files;
    let promises = [];
    let totalSize = 0;

    for (let i = 0; i < files.length; i++) {
        let file = files[i];
        totalSize += file.size;

        if (totalSize > 2600000) {
            Swal.fire({
                icon: 'error',
                title: "<h5 style='color:white'>" + 'Tamanho total excedido!' + "</h5>",
                html: "<h5 style='color:white'>" + 'O tamanho total dos arquivos não pode exceder 2,46 MB por requisição.<br> Por favor, tente novamente com menos arquivos.' + "</h5>",
                showCancelButton: false,
                showConfirmButton: false,
                width: 600,
                padding: '3em',
                color: '#fffff',
                background: '#161a1d'
            })
            return;
        }

        let promise = new Promise((resolve, reject) => {
            let reader = new FileReader();
            reader.readAsText(file);
            reader.onload = (event) => {
                let name = file.name;
                let content = event.target.result;
                resolve({ FileName: name, Content: content });
            };
            reader.onerror = reject;
        });
        promises.push(promise);
    };

    Promise.all(promises)
        .then(contentArray => {
            var table = document.getElementById("tabelaIndex");
            var rows = table.getElementsByTagName("tr");
            var rowsNumber = rows.length;
            var url = window.location.origin;
            $.ajax({
                url: `${url}/Calculator/UploadFile`,
                type: 'POST',
                traditional: true,
                data: { json: JSON.stringify(contentArray) },
                success: (datas) => {
                    var table = $("#fileTable");
                    var counter = rowsNumber;
                    $.each(datas, function (i, data) {
                        var row = $('<tr>');
                        var checkbox = $('<input>').addClass('form-check-input').attr('type', 'checkbox').val('').attr('id', 'flexCheckDefault');
                        checkbox.on('change', function () {
                            toggleStrike(this);
                        });
                        row.append($('<td>').append(checkbox));
                        row.append($('<td>').text(data.layout.viewName));
                        row.append($('<td>').text(data.fileName));
                        row.append($('<td>').text(data.quantity));
                        table.find('tbody').append(row);
                        counter++;
                        if (counter % 2 == 1) {
                            row.addClass('oddRow');
                        }
                    });
                    $("#dropzone").hide();
                    $("#fileTable").show();
                    changeTotalFiles();
                    changeTotalLabels();
                },
                error: () => {
                    Swal.fire({
                        icon: 'error',
                        title: "<h5 style='color:white'>" + 'Erro na requisição!' + "</h5>",
                        html: "<h5 style='color:white'>" + 'Houve um erro na sua requisição. Entre em contato com os desenvolvedores do site e explique sua situação.' + "</h5>",
                        showCancelButton: false,
                        showConfirmButton: false,
                        width: 600,
                        padding: '3em',
                        color: '#fffff',
                        background: '#161a1d'
                    })
                }
            });
        })
        .catch(error => console.error(error));
});

$('#totalLabels, #totalFiles').on('mouseenter', function () {
    const content = "Feitos / A Fazer / Total";
    const popover = new bootstrap.Popover(this, {
        content: content,
        placement: 'top',
    });
    popover.show();

    $(this).on('mouseleave', function () {
        popover.hide();
    });
});

function sortTable() {
    var table = $(this).parents("table").eq(0);
    var colIndex = $(this).index();
    var dataType = $(this).data("type");
    var rows = table.find("tbody tr").get();

    rows.sort(function (rowA, rowB) {
        var cellA = $(rowA).children("td").eq(colIndex);
        var cellB = $(rowB).children("td").eq(colIndex);
        var valA, valB;

        if (dataType === "number") {
            valA = parseInt(cellA.text().trim());
            valB = parseInt(cellB.text().trim());
        } else if (dataType === "string") {
            valA = cellA.text().trim();
            valB = cellB.text().trim();
        }

        if (valA < valB) return -1;
        if (valA > valB) return 1;
        return 0;
    });

    if ($(this).hasClass("asc")) {
        rows.reverse();
        $(this).removeClass("asc").addClass("desc");
    } else {
        $(this).removeClass("desc").addClass("asc");
    }

    $.each(rows, function (index, row) {
        table.children("tbody").append(row);
    });

    paintOddRows();
}