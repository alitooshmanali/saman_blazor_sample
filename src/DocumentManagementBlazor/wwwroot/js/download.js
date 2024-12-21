function downloadFile(fileName, fileExtension, base64Content) {
    const link = document.createElement("a");
    const mimeType = getMimeType(fileExtension);

    link.href = `data:${mimeType};base64,${base64Content}`;
    link.download = fileName + fileExtension;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

function getMimeType(extension) {
    switch (extension.toLowerCase()) {
        case '.pdf':
            return 'application/pdf';
        case '.jpg':
        case '.jpeg':
            return 'image/jpeg';
        case '.png':
            return 'image/png';
        case '.txt':
            return 'text/plain';
        case '.zip':
            return 'application/zip';
        default:
            return 'application/octet-stream';
    }
}