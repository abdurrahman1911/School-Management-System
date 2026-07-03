function loadTable() {
    const classId = document.getElementById("gradeSelect").value;
    const container = document.getElementById("tableContainer");
    const uploadSection = document.getElementById("mainUploadSection");

    if (!classId) {
        container.innerHTML = '<div>برجاء اختيار الصف أولاً</div>';
        uploadSection.style.display = "none";
        return;
    }

    container.innerHTML = '<p>جاري التحميل...</p>';
    uploadSection.style.display = "none";

    fetch(`/Supervisor/GetStudentTable?classId=${classId}&t=${new Date().getTime()}`)
        .then(response => response.text())
        .then(html => {
            if (html.trim() === "") {
                container.innerHTML = '<div>لا يوجد جدول حالياً</div>';
                uploadSection.style.display = "block";
                document.getElementById("uploadTitle").innerText = "إضافة جدول جديد";
            } else {
                container.innerHTML = html;
                container.style.display = "block";
                uploadSection.style.display = "none";
            }
        })
        .catch(err => console.error("Error:", err));
}

function showUploadSection() {
    const container = document.getElementById("tableContainer");
    const uploadSection = document.getElementById("mainUploadSection");
    const title = document.getElementById("uploadTitle");

    if (container) container.style.display = "none";
    if (uploadSection) uploadSection.style.display = "block";
    if (title) title.innerText = "تعديل الجدول الحالي";
}

function handleAreaClick(e) {
    document.getElementById('fileInput').click();
}

function handleFileSelect(input) {
    if (input.files && input.files[0]) {
        const reader = new FileReader();
        reader.onload = function (e) {
            document.getElementById('imagePreview').src = e.target.result;
            document.getElementById('previewContainer').style.display = 'block';
            document.getElementById('uploadPlaceholder').style.display = 'none';
            document.getElementById('saveAction').style.display = 'block';
        }
        reader.readAsDataURL(input.files[0]);
    }
}

function uploadTable() {
    const classId = document.getElementById("gradeSelect").value;
    const fileInput = document.getElementById("fileInput");
    const saveBtn = document.querySelector(".btn-save");

    if (!fileInput.files[0]) {
        alert("يرجى اختيار صورة أولاً");
        return;
    }

    const formData = new FormData();
    formData.append("file", fileInput.files[0]);
    formData.append("classId", classId);

    const originalText = saveBtn.innerText;
    saveBtn.innerText = "جاري الحفظ...";
    saveBtn.disabled = true;

    fetch('/Supervisor/UploadTable', {
        method: 'POST',
        body: formData
    })
        .then(res => res.json())
        .then(data => {
            if (data.success) {
                resetUI();
                loadTable();
                alert("تم التعديل والحفظ بنجاح");
            } else {
                alert("فشل الحفظ: " + data.message);
            }
        })
        .catch(err => {
            console.error("Error:", err);
            alert("حدث خطأ في الاتصال بالسيرفر");
        })
        .finally(() => {
            saveBtn.innerText = originalText;
            saveBtn.disabled = false;
        });
}

function cancelUpload() {
    resetUI();
    loadTable();
}

function resetUI() {
    document.getElementById('fileInput').value = '';
    document.getElementById('previewContainer').style.display = 'none';
    document.getElementById('uploadPlaceholder').style.display = 'block';
    document.getElementById('saveAction').style.display = 'none';
}