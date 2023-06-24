var url = window.location.origin;

// Delete Unit
const deleteUnit = (idParam, nome) => {
	msgModalDel(nome, () => {
		$.ajax({
			url: `${url}/Unit/Delete`,
			method: 'POST',
			data: {
				id: idParam
			},
			success: (resp) => {
				if (resp.code == '200') {
					Swal.fire({
						icon: 'success',
						title: "<h5 style='color:white'>" + `A unidade ${nome} foi excluída.` + "</h5>",
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