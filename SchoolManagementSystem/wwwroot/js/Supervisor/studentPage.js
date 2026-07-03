function loadFilteredData() {
    const stageSelect = document.getElementById('stageSelect');
    const gradeSelect = document.getElementById('gradeSelect');
    const tableBody = document.getElementById('studentsTable');

    const stageId = stageSelect.value;
    const classId = gradeSelect.value;

    if (tableBody) {
        tableBody.innerHTML = '<tr><td style="text-align:center;">جاري التحميل...</td></tr>';
    }

    const url = `/Supervisor/StudentPage?stageId=${stageId}&classId=${classId}`;

    fetch(url, {
        headers: { 'X-Requested-With': 'XMLHttpRequest' }
    })
        .then(response => {
            if (!response.ok) throw new Error('Network response was not ok');
            return response.text();
        })
        .then(html => {
            const parser = new DOMParser();
            const doc = parser.parseFromString(html, 'text/html');

            const newGradeSelect = doc.getElementById('gradeSelect');
            if (newGradeSelect && gradeSelect) {
                gradeSelect.innerHTML = newGradeSelect.innerHTML;

                gradeSelect.value = "";
            }

            const newTableContainer = doc.getElementById('tableContainer');
            const currentTableContainer = document.getElementById('tableContainer');
            if (newTableContainer && currentTableContainer) {
                currentTableContainer.innerHTML = newTableContainer.innerHTML;
            }
        })
        .catch(error => {
            console.error('Error:', error);
            if (tableBody) {
                tableBody.innerHTML = '<tr><td style="text-align:center; color:red;">حدث خطأ في تحديث البيانات</td></tr>';
            }
        });
}

window.onload = function () {
    console.log("System Ready");
};