// Students data
let studentsData = [
    { id: 1, name: "محمد احمد السيد", number: "2024001", fatherNumber: "0501234567", attendance: 0, performance: "excellent", grade: 92, stage: "primary", class: "6" },
    { id: 2, name: "فاطمة محمود حسن", number: "2024002", fatherNumber: "0502345678", attendance: 1, performance: "excellent", grade: 96, stage: "primary", class: "6" },
    { id: 3, name: "خالد السيد محمد", number: "2024003", fatherNumber: "0503456789", attendance: 2, performance: "good", grade: 75, stage: "primary", class: "5" },
    { id: 4, name: "مارن محمد عبدالرحمن", number: "2024004", fatherNumber: "0504567890", attendance: 3, performance: "very-good", grade: 88, stage: "middle", class: "8" },
    { id: 5, name: "روان محمد ياسر", number: "2024005", fatherNumber: "0505678901", attendance: 5, performance: "excellent", grade: 94, stage: "secondary", class: "10" },
    { id: 7, name: "سارة محمود ابراهيم", number: "2024007", fatherNumber: "0507890123", attendance: 0, performance: "very-good", grade: 90, stage: "middle", class: "3" },
    { id: 8, name: "عمر عبدالله حسن", number: "2024008", fatherNumber: "0508901234", attendance: 3, performance: "good", grade: 71, stage: "middle", class: "7" },
    { id: 9, name: "ليلى محمد سعيد", number: "2024009", fatherNumber: "0509012345", attendance: 1, performance: "excellent", grade: 95, stage: "secondary", class: "11" },
    { id: 10, name: "يوسف علي كريم", number: "2024010", fatherNumber: "0510123456", attendance: 0, performance: "very-good", grade: 89, stage: "secondary", class: "12" }
];

// بيانات الصفوف حسب المرحلة (مع إضافة خيار الكل)
const classesByStage = {
    
    primary: [
        { value: "all", text: "جميع الصفوف" },
        { value: "1", text: "الصف الأول" },
        { value: "2", text: "الصف الثاني" },
        { value: "3", text: "الصف الثالث" },
        { value: "4", text: "الصف الرابع" },
        { value: "5", text: "الصف الخامس" },
        { value: "6", text: "الصف السادس" }
    ],
    middle: [
        { value: "all", text: "جميع الصفوف" },
        { value: "7", text: "الصف الاول المتوسط" },
        { value: "8", text: "الصف الثاني المتوسط" },
        { value: "9", text: "الصف الثالث المتوسط" }
    ],
    secondary: [
        { value: "all", text: "جميع الصفوف" },
        { value: "10", text: "الصف الاول الثانوي" },
        { value: "11", text: "الصف الثاني الثانوي " },
        { value: "12", text: "الصف الثالث الثانوي " }
    ]
};

// Update date
function updateDate() {
    const dateElement = document.getElementById('currentDate');
    if (dateElement) {
        const now = new Date();
        const arabicDate = now.toLocaleDateString('ar-EG', {
            weekday: 'long',
            year: 'numeric',
            month: 'long',
            day: 'numeric'
        });
        dateElement.textContent = arabicDate;
    }
}

// تحديث قائمة الصفوف بناءً على المرحلة المختارة
function updateClassOptions() {
    const stageSelect = document.getElementById('stageSelect');
    const classSelect = document.getElementById('classSelect');

    if (!stageSelect || !classSelect) return;

    const selectedStage = stageSelect.value;

    // تفريغ القائمة
    classSelect.innerHTML = '';

    // إضافة "جميع الصفوف"
    const allOption = document.createElement('option');
    allOption.value = 'all';
    allOption.textContent = 'جميع الصفوف';
    classSelect.appendChild(allOption);

    // لو اختار مرحلة معينة
    if (selectedStage !== 'all') {

        const classes = classesByStage[selectedStage] || [];

        classes.forEach(cls => {

            // عشان ما يكررش "جميع الصفوف"
            if (cls.value !== 'all') {

                const option = document.createElement('option');
                option.value = cls.value;
                option.textContent = cls.text;

                classSelect.appendChild(option);
            }
        });
    }

    // القيمة الافتراضية
    classSelect.value = 'all';
}

// تحديث جدول الطلاب مع الفلتر
function updateStudentsTable() {
    const tableBody = document.getElementById('studentsTableBody');
    if (!tableBody) return;

    // الحصول على قيم الفلتر
    const stageFilter = document.getElementById('stageSelect')?.value;
    const classFilter = document.getElementById('classSelect')?.value;

    // تصفية البيانات
    let filteredData = studentsData;

    // تصفية حسب المرحلة
   if (stageFilter && stageFilter !== 'all') {
    filteredData = filteredData.filter(student => student.stage === stageFilter);
}

if (classFilter && classFilter !== 'all') {
    filteredData = filteredData.filter(student => student.class === classFilter);
}

    // تصفية حسب الصف (إذا لم يكن "الكل")
    if (classFilter && classFilter !== 'all') {
        filteredData = filteredData.filter(student => student.class === classFilter);
    }

    tableBody.innerHTML = '';

    if (filteredData.length === 0) {
        // عرض رسالة في حالة عدم وجود بيانات
        const row = document.createElement('tr');
        row.innerHTML = `<td colspan="5" style="text-align: center; padding: 40px; color: #6c757d;">📚 لا توجد طلاب في هذه المرحلة / الصف</td>`;
        tableBody.appendChild(row);
        return;
    }

    filteredData.forEach(student => {
        // Determine performance color
        let performanceClass = '';
        let performanceText = '';

        switch (student.performance) {
            case 'excellent':
                performanceClass = 'excellent';
                performanceText = 'ممتاز';
                break;
            case 'very-good':
                performanceClass = 'very-good';
                performanceText = 'جيد جداً';
                break;
            case 'good':
                performanceClass = 'good';
                performanceText = 'جيد';
                break;
            default:
                performanceClass = 'good';
                performanceText = 'جيد';
        }

        // Determine attendance color
        const attendanceClass = student.attendance >= 0 ? 'present' : 'absent';

        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${student.name}</td>
            <td><span class="status ${attendanceClass}">${student.attendance}</span></td>
            <td><span class="performance ${performanceClass}">${performanceText}</span></td>
            <td><span class="grade">${student.grade}%</span></td>
            <td>
                <div class="action-buttons">
                    <button class="action-btn view-btn" onclick="showStudentDetails(${student.id})">
                        عرض البيانات
                    </button>
                    <button class="action-btn attendance-btn" onclick="showAttendance(${student.id})">
                        الحضور
                    </button>
                    <button class="action-btn grades-btn" onclick="showGrades(${student.id})">
                        الدرجات
                    </button>
                </div>
             </td>
        `;

        tableBody.appendChild(row);
    });
}

// تطبيق الفلتر مع تأثير تحميل
function applyFilters() {
    const applyBtn = document.getElementById('applyFiltersBtn');

    if (applyBtn) {
        // إضافة تأثير التحميل
        applyBtn.classList.add('btn-loading');
        applyBtn.textContent = 'جاري التطبيق...';

        // تعطيل القوائم مؤقتًا
        const stageSelect = document.getElementById('stageSelect');
        const classSelect = document.getElementById('classSelect');
        if (stageSelect) stageSelect.disabled = true;
        if (classSelect) classSelect.disabled = true;

        // محاكاة وقت التحميل
        setTimeout(() => {
            updateStudentsTable();

            // الحصول على أسماء الفلتر المختارة لعرضها في الكونسول
            const stageSelect = document.getElementById('stageSelect');
            const classSelect = document.getElementById('classSelect');
            const stageNames = {
                'primary': 'المرحلة الإبتدائية',
                'middle': 'المرحلة المتوسطة',
                'secondary': 'المرحلة الثانوية'
            };

            const classText = classSelect?.options[classSelect.selectedIndex]?.text || '';
            const stageName = stageNames[stageSelect?.value] || '';

            console.log(`✅ تم تطبيق الفلتر: ${stageName} - ${classText}`);

            // إزالة تأثير التحميل
            applyBtn.classList.remove('btn-loading');
            applyBtn.textContent = 'تطبيق الفلتر';

            // إعادة تفعيل القوائم
            if (stageSelect) stageSelect.disabled = false;
            if (classSelect) classSelect.disabled = false;
        }, 300);
    } else {
        updateStudentsTable();
    }
}

// Display student details
function showStudentDetails(studentId) {
    const student = studentsData.find(s => s.id === studentId);
    if (!student) return;

    document.getElementById('detailName').value = student.name;
    document.getElementById('detailNumber').value = student.number;
    document.getElementById('detailFatherNumber').value = student.fatherNumber;
    document.getElementById('detailGrade').value = student.grade + '%';

    document.getElementById('studentDetailsForm').style.display = 'flex';
}

// Display attendance
function showAttendance(studentId) {
    const student = studentsData.find(s => s.id === studentId);
    if (!student) return;

    const TOTAL_DAYS = 5; //  لازم يتعرف

    const absenceDays = student.attendance;
    const presentDays = TOTAL_DAYS - absenceDays;

    const days = ['الأحد', 'الإثنين', 'الثلاثاء', 'الأربعاء', 'الخميس'];

    let attendanceHTML = `
        <h4 style="text-align: center; color: #4387AB; margin-bottom: 20px;">
            حضور ${student.name}
        </h4>

        <div class="attendance-days">
    `;

   

days.forEach((day, index) => {
    const isAbsent = index < absenceDays;

    attendanceHTML += `
        <div class="day-attendance">
            <div class="day-name">${day}</div>
            <div class="att-status ${isAbsent ? 'absent' : 'present'}">
                ${isAbsent ? ' غياب' : ' حضور'}
            </div>
        </div>
    `;
});

    attendanceHTML += `
        </div>

        <div style="text-align: center; padding: 15px; background: #f8f9fa; border-radius: 6px; margin-top: 20px;">
            <strong>عدد أيام الغياب : ${absenceDays}</strong>
        </div>
    `;

    document.getElementById('attendanceContent').innerHTML = attendanceHTML;
    document.getElementById('attendanceForm').style.display = 'flex';
}

// Display grades
function showGrades(studentId) {
    const student = studentsData.find(s => s.id === studentId);
    if (!student) return;

    // Subject grades based on student's overall grade
    const baseGrade = student.grade;
    const subjects = [
        { name: 'الرياضيات', grade: Math.min(100, Math.max(60, baseGrade + (Math.random() * 10 - 5))) },
        { name: 'العلوم', grade: Math.min(100, Math.max(60, baseGrade + (Math.random() * 10 - 5))) },
        { name: 'اللغة العربية', grade: Math.min(100, Math.max(60, baseGrade + (Math.random() * 10 - 5))) },
        { name: 'اللغة الإنجليزية', grade: Math.min(100, Math.max(60, baseGrade + (Math.random() * 10 - 5))) },
        { name: 'التربية الإسلامية', grade: Math.min(100, Math.max(60, baseGrade + (Math.random() * 10 - 5))) },
        { name: 'الاجتماعيات', grade: Math.min(100, Math.max(60, baseGrade + (Math.random() * 10 - 5))) }
    ];

    let gradesHTML = `
        <h4 style="text-align: center; color: #4387AB; margin-bottom: 20px;">درجات ${student.name}</h4>
        <div class="grades-list">
    `;

    subjects.forEach(subject => {
        const roundedGrade = Math.round(subject.grade);
        gradesHTML += `
            <div class="subject-grade">
                <span class="subject-name">${subject.name}</span>
                <span class="subject-score">${roundedGrade}%</span>
            </div>
        `;
    });

    gradesHTML += `
        </div>
        <div style="text-align: center; padding: 15px; background: #f8f9fa; border-radius: 6px; margin-top: 20px;">
            <strong>المعدل العام: ${student.grade}%</strong>
        </div>
    `;

    document.getElementById('gradesContent').innerHTML = gradesHTML;
    document.getElementById('gradesForm').style.display = 'flex';
}

// Close forms
function setupFormEvents() {
    // Close student details form
    const closeDetailsBtn = document.getElementById('closeDetailsBtn');
    if (closeDetailsBtn) {
        closeDetailsBtn.addEventListener('click', function () {
            document.getElementById('studentDetailsForm').style.display = 'none';
        });
    }

    // Close attendance form
    const closeAttendanceBtn = document.getElementById('closeAttendanceBtn');
    if (closeAttendanceBtn) {
        closeAttendanceBtn.addEventListener('click', function () {
            document.getElementById('attendanceForm').style.display = 'none';
        });
    }

    // Close grades form
    const closeGradesBtn = document.getElementById('closeGradesBtn');
    if (closeGradesBtn) {
        closeGradesBtn.addEventListener('click', function () {
            document.getElementById('gradesForm').style.display = 'none';
        });
    }

    // Close on click outside
    const popups = document.querySelectorAll('.form-popup');
    popups.forEach(popup => {
        popup.addEventListener('click', function (e) {
            if (e.target === this) {
                this.style.display = 'none';
            }
        });
    });
}

// إعداد أحداث الفلتر
function setupFilterEvents() {
    const stageSelect = document.getElementById('stageSelect');
    const applyBtn = document.getElementById('applyFiltersBtn');

    // تحديث الصفوف عند تغيير المرحلة
    if (stageSelect) {
        stageSelect.addEventListener('change', function () {
            updateClassOptions();
        });
    }

    // تطبيق الفلتر عند الضغط على الزر
    if (applyBtn) {
        applyBtn.addEventListener('click', function (e) {
            e.preventDefault();
            applyFilters();
        });
    }

    // اختصار لوحة المفاتيح Ctrl + Enter لتطبيق الفلتر
    document.addEventListener('keydown', function (e) {
        if ((e.ctrlKey || e.metaKey) && e.key === 'Enter') {
            e.preventDefault();
            applyFilters();
        }
    });
}

// إضافة وظيفة لعرض إحصائيات سريعة
function showStatistics() {
    const stageSelect = document.getElementById('stageSelect');
    const classSelect = document.getElementById('classSelect');

    const stageFilter = stageSelect?.value;
    const classFilter = classSelect?.value;

    let filteredData = studentsData;

    if (stageFilter) {
        filteredData = filteredData.filter(student => student.stage === stageFilter);
    }

    if (classFilter && classFilter !== 'all') {
        filteredData = filteredData.filter(student => student.class === classFilter);
    }

    const totalStudents = filteredData.length;
    const avgAttendance = totalStudents > 0 ?
        Math.round(filteredData.reduce((sum, s) => sum + s.attendance, 0) / totalStudents) : 0;
    const avgGrade = totalStudents > 0 ?
        Math.round(filteredData.reduce((sum, s) => sum + s.grade, 0) / totalStudents) : 0;

    console.log(`📊 إحصائيات: ${totalStudents} طالب | متوسط الحضور: ${avgAttendance}% | متوسط الدرجات: ${avgGrade}%`);
}

// Page initialization
document.addEventListener('DOMContentLoaded', function () {
    updateDate();
    updateClassOptions(); // تهيئة الصفوف مع خيار "جميع الصفوف"
    updateStudentsTable(); // عرض البيانات الأولية
    setupFormEvents();
    setupFilterEvents();
    showStatistics(); // عرض إحصائيات أولية
});