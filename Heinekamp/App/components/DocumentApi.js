export const getDocumentListApi = async () => await fetch(`/api/document/all`, {
    method: 'GET',
    headers: {
        'Content-Type': 'application/json'
    }
});

export const getFileTypesApi = async () => await fetch('/api/document/fileTypes', {
    method: 'GET',
    headers: {
        'Content-Type': 'application/json'
    }
});

export const postUploadDocsApi = async (formData) => await fetch('/api/document/create', {
    method: 'POST',
    body: formData
});

export const postUpdateDocApi = async (updateData) => await fetch('/api/document/update', {
    method: 'POST',
    headers: {
        'Content-Type': 'application/json'
    },
    body: JSON.stringify(updateData)
});

export const postDeleteDocApi = async (id) => await fetch(`/api/document/delete/${id}`, {
    method: 'POST',
    headers: {
        'Content-Type': 'application/json'
    }
});