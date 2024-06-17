function confirmDeleteDR(event) {
    event.preventDefault();
    const confirmed = confirm("¿Está seguro de que desea eliminar este doctor?");
    if (confirmed) {
        alert('Doctor eliminado');
      
    }
}
function confirmDelete(event) {
    event.preventDefault();
    const confirmed = confirm("¿Está seguro de que desea eliminar esta sala?");
    if (confirmed) {
        alert('Doctor eliminado');
      
    }
}
