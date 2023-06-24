var url = window.location.origin;

//Deletar um layout
const deleteLayout = (idParam, name) => {
	msgModalDel(name, () => {
		$.ajax({
			url: `${url}/Calculator/Delete`,
			method: 'POST',
			data: {
				id: idParam
			},
			success: (resp) => {
				if (resp.code == '200') {
					Swal.fire({
						icon: 'success',
						title: "<h5 style='color:white'>" + `O layout "${name}" foi deletado!` + "</h5>",
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
};