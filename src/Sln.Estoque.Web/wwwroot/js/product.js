var url = window.location.origin;

// Deletar Produto
const deleteProduct = (idParam, name) => {
	msgModalDel(name, () => {
		$.ajax({
			url: `${url}/Product/Delete`,
			method: 'POST',
			data: {
				id: idParam
			},
			success: (resp) => {
				if (resp.code == '200') {
					Swal.fire({
						icon: 'success',
						title: "<h5 style='color:white'>" + `O produto "${name}" foi excluído!` + "</h5>",
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

// Atualizar Quantidade
const updtQty = (idParam, quantity) => {
	msgModalUpdt(idParam, quantity, () => {
		closeMsgModalMessage();
	});
};


msgModalUpdt = (idParam, qtyAtual) => {
	let contentBody = '<span class="p-2 text-white">Insira a quantidade que deseja alterar no estoque!</span><br/><input id="input-modal-qty" class="mt-3" type="text" placeholder="Insira um valor" onkeyup="onlyTwoDigits(event)">';

	let contentFooter =
		`<button id="btnModalAdd" type="button" class="btn btn-success" data-bs-dismiss="modal" onclick="addValue(${idParam}, ${qtyAtual})">Adicionar</button> 
        <button id="btnModalRem" type="button" class="btn btn-danger" data-bs-dismiss="modal" onclick="minusValue(${idParam}, ${qtyAtual})">Remover</button>`;

	$('#modal-body').html(contentBody);
	$('#footer-modal').html(contentFooter);

	$('#msgModal').modal('show');
}

function onlyTwoDigits(event) {
	let value = event.target.value;
	value = value.replace(/[^0-9,.]/g, '');
	value = value.replace(/,/g, '.');
	if (value.indexOf('.') !== -1) {
		let parts = value.split('.');
		let decimalPart = parts[1];
		if (decimalPart && decimalPart.length > 2) {
			decimalPart = decimalPart.slice(0, 2);
		}
		value = parts[0] + '.' + (decimalPart ? decimalPart : '');
	}
	event.target.value = value;
}
