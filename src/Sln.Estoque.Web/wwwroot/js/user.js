var url = window.location.origin;

// Deletar Usuário
const deleteUser = (idParam, nome) => {
    msgModalDel(nome, () => {
        $.ajax({
            url: `${url}/User/Delete`,
            method: 'POST',
            data: {
                id: idParam
            },
            success: (resp) => {
                if (resp.status == 'success') {
                    Swal.fire({
                        icon: 'success',
                        title: "<h5 style='color:white'>" + `Usuário "${nome}" foi excluído.` + "</h5>",
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
                        title: "<h5 style='color:white'>" + `Não foi possível excluir o usuário` + "</h5>",
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
        closeMsgModalMessage();
    });
}

const modalPassword = () => {
    msgModalPassword(() => {
        closeMsgModalMessage();
    });
}