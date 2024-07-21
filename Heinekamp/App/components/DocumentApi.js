const getPageApi = async (pageNumber) => await fetch('/api/document/page', {
    method: 'GET',
    headers: {
        'Content-Type': 'application/json'
    },
    body: JSON.stringify({pageNumber})
});

const getFileTypesApi = async () => await fetch('/api/document/fileTypes', {
    method: 'GET',
    headers: {
        'Content-Type': 'application/json'
    }
});

const postUploadDocsApi = async (formData) => await fetch('/api/documents/create', {
    method: 'POST',
    body: formData
});

const postUpdateDocApi = async (updateData) => await fetch('/api/document/update', {
    method: 'POST',
    headers: {
        'Content-Type': 'application/json'
    },
    body: JSON.stringify({updateData})
});

const postDeleteDocApi = async (id) => await fetch('/api/document/delete', {
    method: 'POST',
    headers: {
        'Content-Type': 'application/json'
    },
    body: JSON.stringify({id})
});