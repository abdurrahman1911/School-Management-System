    function loadTeacherTable() {
        const teacherId = document.getElementById("teacherSelect").value;
        const container = document.getElementById("teacherTableContainer");
        const uploadSection = document.getElementById("teacherUploadSection");
        const title = document.getElementById("tableTitle");

        if (!teacherId) {
            container.innerHTML = '<div class="no-table"><i class="fas fa-folder-open"></i><p>برجاء اختيار معلم لعرض الجدول</p></div>';
            uploadSection.style.display = "none";
            title.innerText = "جدول المعلم :";
            return;
        }

        const teacherName = document.getElementById("teacherSelect").options[document.getElementById("teacherSelect").selectedIndex].text;
        title.innerText = `جدول المعلم ${teacherName} :`;

        container.innerHTML = '<p style="text-align:center;">جاري التحميل...</p>';
        uploadSection.style.display = "none";

        fetch(`/Supervisor/GetTeacherTable?teacherId=${teacherId}&t=${new Date().getTime()}`)
            .then(response => response.text())
            .then(html => {
                if (html.trim() === "") {
                    container.innerHTML = '<div class="no-table"><i class="fas fa-folder-open"></i><p>لا يوجد جدول لهذا المعلم حالياً</p></div>';
                    uploadSection.style.display = "block";
                    document.getElementById("teacherUploadTitle").innerText = "إضافة جدول جديد للمعلم";
                } else {
                    container.innerHTML = html;
                    container.style.display = "block";
                    uploadSection.style.display = "none";
                }
            })
            .catch(err => console.error("Error:", err));
    }

    function showTeacherUploadSection() {
        document.getElementById("teacherTableContainer").style.display = "none";
        const uploadSection = document.getElementById("teacherUploadSection");
        uploadSection.style.display = "block";
        document.getElementById("teacherUploadTitle").innerText = "تعديل الجدول الحالي";
    }

    function handleTeacherAreaClick() {
        document.getElementById('teacherFileInput').click();
    }

    function handleTeacherFileSelect(input) {
        if (input.files && input.files[0]) {
            const reader = new FileReader();
            reader.onload = function (e) {
                document.getElementById('teacherImagePreview').src = e.target.result;
                document.getElementById('teacherPreviewContainer').style.display = 'block';
                document.getElementById('teacherUploadPlaceholder').style.display = 'none';
                document.getElementById('teacherSaveAction').style.display = 'block';
            }
            reader.readAsDataURL(input.files[0]);
        }
    }

    function uploadTeacherTable() {
        const teacherId = document.getElementById("teacherSelect").value;
        const fileInput = document.getElementById("teacherFileInput");
        const saveBtn = document.querySelector(".btn-save-teacher");

        if (!fileInput.files[0]) {
            alert("يرجى اختيار صورة أولاً");
            return;
        }

        const formData = new FormData();
        formData.append("file", fileInput.files[0]);
        formData.append("teacherId", teacherId);

        const originalText = saveBtn.innerHTML;
        saveBtn.innerHTML = "جاري الحفظ...";
        saveBtn.disabled = true;

        fetch('/Supervisor/UploadTeacherTable', {
            method: 'POST',
            body: formData
        })
            .then(res => res.json())
            .then(data => {
                if (data.success) {
                    resetTeacherUI();
                    loadTeacherTable();
                    alert("تم حفظ جدول المعلم بنجاح ✅");
                } else {
                    alert("فشل الحفظ: " + data.message);
                }
            })
            .catch(err => {
                console.error("Error:", err);
                alert("حدث خطأ في الاتصال بالسيرفر");
            })
            .finally(() => {
                saveBtn.innerHTML = originalText;
                saveBtn.disabled = false;
            });
    }

    function resetTeacherUI() {
        document.getElementById('teacherFileInput').value = '';
        document.getElementById('teacherPreviewContainer').style.display = 'none';
        document.getElementById('teacherUploadPlaceholder').style.display = 'block';
        document.getElementById('teacherSaveAction').style.display = 'none';
    }