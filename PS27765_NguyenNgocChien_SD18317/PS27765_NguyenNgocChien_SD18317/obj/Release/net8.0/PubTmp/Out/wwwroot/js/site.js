// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/*async function readImageFromUrl() {
    var imageUrl = document.getElementById('imageUrl').value;

    if (!imageUrl) {
        // Nếu đường dẫn URL rỗng, loại bỏ hình ảnh
        document.getElementById('imageContainer').innerHTML = '';
        return;
    }

    try {
        // Tải hình ảnh từ đường dẫn trên cloud
        const response = await fetch(imageUrl);
        const blob = await response.blob();
        const urlObject = URL.createObjectURL(blob);

        // Hiển thị hình ảnh
        document.getElementById('imageContainer').innerHTML = `<img src="${urlObject}" alt="Image from Cloud">`;

        // Đọc hình ảnh thành văn bản
        readImageToText(urlObject);
    } catch (error) {
        console.error('Error: ', error);
    }
}

function readImageToText(imageDataUrl) {
    Tesseract.recognize(
        imageDataUrl,
        'eng', // Ngôn ngữ được nhận dạng
        { logger: m => console.log(m) } // Logger, bạn có thể loại bỏ nếu không cần
    ).then(({ data: { text } }) => {
        // Hiển thị văn bản được nhận dạng từ hình ảnh
        alert('Text from image: ' + text);
    });
}

// Gắn sự kiện input vào trường nhập liệu
document.getElementById('imageUrl').addEventListener('input', readImageFromUrl);*/
