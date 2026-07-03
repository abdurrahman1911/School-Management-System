// ================= بيانات  =================
const studentsData = {
    Kindergarten: {
        1: [
            { id: 1, name: "محمد محمود عبدالرحيم", grades: { math: { midterm1: 28, final1: 29, midterm2: 27, final2: 30 }, arabic: { midterm1: 27, final1: 28, midterm2: 26, final2: 29 }, english: { midterm1: 25, final1: 27, midterm2: 24, final2: 28 }, science: { midterm1: 26, final1: 28, midterm2: 25, final2: 29 } }, attendance: 94 },
            { id: 2, name: "خالد السيد محمد", grades: { math: { midterm1: 25, final1: 27, midterm2: 24, final2: 28 }, arabic: { midterm1: 24, final1: 26, midterm2: 23, final2: 27 }, english: { midterm1: 22, final1: 24, midterm2: 21, final2: 25 }, science: { midterm1: 23, final1: 25, midterm2: 22, final2: 26 } }, attendance: 85 },
            { id: 3, name: "مازن محمد عبدالعظيم", grades: { math: { midterm1: 30, final1: 30, midterm2: 29, final2: 30 }, arabic: { midterm1: 29, final1: 30, midterm2: 28, final2: 30 }, english: { midterm1: 28, final1: 29, midterm2: 27, final2: 30 }, science: { midterm1: 30, final1: 30, midterm2: 29, final2: 30 } }, attendance: 98 },
            { id: 4, name: "صالح عبدالله محمد", grades: { math: { midterm1: 26, final1: 27, midterm2: 25, final2: 28 }, arabic: { midterm1: 25, final1: 26, midterm2: 24, final2: 27 }, english: { midterm1: 23, final1: 25, midterm2: 22, final2: 26 }, science: { midterm1: 24, final1: 26, midterm2: 23, final2: 27 } }, attendance: 87 }
        ]
    },
    primary: {
        1: [
            { id: 5, name: "محمد أحمد السيد", grades: { math: { midterm1: 28, final1: 29, midterm2: 27, final2: 30 }, arabic: { midterm1: 25, final1: 28, midterm2: 26, final2: 29 }, english: { midterm1: 22, final1: 25, midterm2: 24, final2: 27 }, science: { midterm1: 26, final1: 28, midterm2: 25, final2: 29 } }, attendance: 92 },
            { id: 6, name: "فاطمة محمود حسن", grades: { math: { midterm1: 29, final1: 30, midterm2: 28, final2: 29 }, arabic: { midterm1: 28, final1: 29, midterm2: 27, final2: 30 }, english: { midterm1: 26, final1: 28, midterm2: 25, final2: 29 }, science: { midterm1: 28, final1: 29, midterm2: 27, final2: 30 } }, attendance: 95 },
            { id: 7, name: "خالد السيد محمد", grades: { math: { midterm1: 25, final1: 27, midterm2: 24, final2: 28 }, arabic: { midterm1: 24, final1: 26, midterm2: 23, final2: 27 }, english: { midterm1: 22, final1: 24, midterm2: 21, final2: 25 }, science: { midterm1: 23, final1: 25, midterm2: 22, final2: 26 } }, attendance: 85 },
            { id: 8, name: "مازن محمد عبدالعظيم", grades: { math: { midterm1: 30, final1: 30, midterm2: 29, final2: 30 }, arabic: { midterm1: 29, final1: 30, midterm2: 28, final2: 30 }, english: { midterm1: 28, final1: 29, midterm2: 27, final2: 30 }, science: { midterm1: 30, final1: 30, midterm2: 29, final2: 30 } }, attendance: 98 },
            { id: 9, name: "روان محمد ياسر", grades: { math: { midterm1: 27, final1: 28, midterm2: 26, final2: 29 }, arabic: { midterm1: 26, final1: 27, midterm2: 25, final2: 28 }, english: { midterm1: 24, final1: 26, midterm2: 23, final2: 27 }, science: { midterm1: 25, final1: 27, midterm2: 24, final2: 28 } }, attendance: 90 }
        ],
        2: [
            { id: 10, name: "فيروز أحمد محمد", grades: { math: { midterm1: 26, final1: 28, midterm2: 25, final2: 29 }, arabic: { midterm1: 25, final1: 27, midterm2: 24, final2: 28 }, english: { midterm1: 23, final1: 25, midterm2: 22, final2: 26 }, science: { midterm1: 24, final1: 26, midterm2: 23, final2: 27 } }, attendance: 88 },
            { id: 11, name: "آمنة محمد عبدالله", grades: { math: { midterm1: 29, final1: 29, midterm2: 28, final2: 30 }, arabic: { midterm1: 28, final1: 29, midterm2: 27, final2: 29 }, english: { midterm1: 26, final1: 28, midterm2: 25, final2: 29 }, science: { midterm1: 27, final1: 29, midterm2: 26, final2: 30 } }, attendance: 93 },
            { id: 12, name: "فريدة أحمد صالح", grades: { math: { midterm1: 27, final1: 28, midterm2: 26, final2: 29 }, arabic: { midterm1: 26, final1: 28, midterm2: 25, final2: 28 }, english: { midterm1: 24, final1: 26, midterm2: 23, final2: 27 }, science: { midterm1: 25, final1: 27, midterm2: 24, final2: 28 } }, attendance: 91 }
        ]
    },
    preparatory: {
        1: [
            { id: 13, name: "محمد محمود عبدالرحيم", grades: { math: { midterm1: 28, final1: 29, midterm2: 27, final2: 30 }, arabic: { midterm1: 27, final1: 28, midterm2: 26, final2: 29 }, english: { midterm1: 25, final1: 27, midterm2: 24, final2: 28 }, science: { midterm1: 26, final1: 28, midterm2: 25, final2: 29 } }, attendance: 94 },
            { id: 14, name: "صالح عبدالله محمد", grades: { math: { midterm1: 26, final1: 27, midterm2: 25, final2: 28 }, arabic: { midterm1: 25, final1: 26, midterm2: 24, final2: 27 }, english: { midterm1: 23, final1: 25, midterm2: 22, final2: 26 }, science: { midterm1: 24, final1: 26, midterm2: 23, final2: 27 } }, attendance: 87 }
        ]
    },
    secondary: {
        1: [
            { id: 15, name: "ابراهيم أحمد محمد", grades: { math: { midterm1: 26, final1: 28, midterm2: 25, final2: 29 }, arabic: { midterm1: 25, final1: 27, midterm2: 24, final2: 28 }, english: { midterm1: 23, final1: 25, midterm2: 22, final2: 26 }, science: { midterm1: 24, final1: 26, midterm2: 23, final2: 27 } }, attendance: 88 },
            { id: 16, name: "ايمان محمد عبدالله", grades: { math: { midterm1: 29, final1: 29, midterm2: 28, final2: 30 }, arabic: { midterm1: 28, final1: 29, midterm2: 27, final2: 29 }, english: { midterm1: 26, final1: 28, midterm2: 25, final2: 29 }, science: { midterm1: 27, final1: 29, midterm2: 26, final2: 30 } }, attendance: 93 },
            { id: 17, name: "محمد أحمد صالح", grades: { math: { midterm1: 27, final1: 28, midterm2: 26, final2: 29 }, arabic: { midterm1: 26, final1: 28, midterm2: 25, final2: 28 }, english: { midterm1: 24, final1: 26, midterm2: 23, final2: 27 }, science: { midterm1: 25, final1: 27, midterm2: 24, final2: 28 } }, attendance: 1 }
        ]
    }
};

const studentsData = studentsData;


const stageFilter = document.getElementById('stageFilter');
const gradeFilter = document.getElementById('gradeFilter');
const studentsList = document.getElementById('studentsList');
const scheduleImage = document.getElementById('scheduleImage');
const noScheduleMsg = document.getElementById('noScheduleMsg');
const studentGradeFilter = document.getElementById('studentGradeFilter');
const subjectFilter = document.getElementById('subjectFilter');
const examFilter = document.getElementById('examFilter');
const gradesTableBody = document.getElementById('gradesTableBody');

let attendanceChart = null;
let currentStudents = [];
let currentSelectedStudent = null;

// ================= أسماء المواد والاختبارات =================
const subjectNames = {
    math: 'الرياضيات',
    arabic: 'اللغة العربية',
    english: 'اللغة الإنجليزية',
    science: 'العلوم'
};

const examNames = {
    midterm1: 'منتصف الفصل الأول',
    final1: 'نهائي الفصل الأول',
    midterm2: 'منتصف الفصل الثاني',
    final2: 'نهائي الفصل الثاني'
};

const examMaxScores = {
    midterm1: 30,
    final1: 30,
    midterm2: 30,
    final2: 30
};

// ================= تحديث قائمة الطلاب =================
function updateStudentsList() {
    const stage = stageFilter.value;
    const grade = gradeFilter.value;

    currentStudents = studentsData[stage]?.[grade] || [];

    studentsList.innerHTML = '';
    studentGradeFilter.innerHTML = '<option value="">اختر الطالب</option>';

    if (currentStudents.length === 0) {
        studentsList.innerHTML = '<li style="text-align:center; color:#888;">لا يوجد طلاب</li>';
        return;
    }

    currentStudents.forEach((student, index) => {
        const li = document.createElement('li');
        li.textContent = student.name;
        li.dataset.id = student.id;
        li.onclick = () => selectStudent(student.id);
        studentsList.appendChild(li);

        const option = document.createElement('option');
        option.value = student.id;
        option.textContent = student.name;
        studentGradeFilter.appendChild(option);
    });

    if (currentStudents.length > 0) {
        selectStudent(currentStudents[0].id);
    }
}

// ================= اختيار طالب =================
function selectStudent(studentId) {
    currentSelectedStudent = currentStudents.find(s => s.id == studentId);

    document.querySelectorAll('.students-list li').forEach(li => {
        li.classList.remove('active');
        if (li.dataset.id == studentId) {
            li.classList.add('active');
        }
    });

    studentGradeFilter.value = studentId;

    updateGradesTable();

    // تحديث الرسم البياني للحضور
    updateAttendanceChart();
}

// =================  جدول الدرجات =================
function updateGradesTable() {
    if (!currentSelectedStudent) {
        gradesTableBody.innerHTML = '<tr><td colspan="6" style="text-align:center;">اختر طالباً أولاً</td></tr>';
        return;
    }

    const subject = subjectFilter.value;
    const exam = examFilter.value;
    const grade = currentSelectedStudent.grades[subject]?.[exam] || 0;
    const maxScore = examMaxScores[exam];
    const percentage = ((grade / maxScore) * 100).toFixed(1);
    const passed = grade >= (maxScore * 0.5);


    gradesTableBody.innerHTML = '';

    currentStudents.forEach((student, index) => {
        const studentGrade = student.grades[subject]?.[exam] || 0;
        const studentPercentage = ((studentGrade / maxScore) * 100).toFixed(1);
        const studentPassed = studentGrade >= (maxScore * 0.5);

        const row = gradesTableBody.insertRow();
        row.innerHTML = `
            <td>${index + 1}</td>
            <td>${student.name}</td>
            <td>${studentGrade}</td>
            <td>${maxScore}</td>
            <td>${studentPercentage}%</td>
            <td class="${studentPassed ? 'status-pass' : 'status-fail'}">${studentPassed ? 'ناجح' : 'راسب'}</td>
        `;

        if (student.id == currentSelectedStudent.id) {
            row.style.background = '#eef3f6';
            row.style.fontWeight = 'bold';
        }
    });
}

// =================  الرسم البياني =================
function updateAttendanceChart() {
    if (!currentSelectedStudent) return;

    const attendance = currentSelectedStudent.attendance || 90;
    const absence = 100 - attendance;

    if (attendanceChart) {
        attendanceChart.destroy();
    }

    const ctx = document.getElementById('attendanceChart').getContext('2d');
    attendanceChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ['حضور', 'غياب'],
            datasets: [{
                data: [attendance, absence],
                backgroundColor: ['#2ecc71', '#e74c3c'],
                borderWidth: 0
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: true,
            plugins: {
                legend: {
                    position: 'bottom',
                    labels: { font: { size: 12 } }
                },
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            return `${context.label}: ${context.raw}%`;
                        }
                    }
                }
            }
        }
    });
}

// =================  الجدول الدراسي =================
function updateScheduleImage() {
    const stage = stageFilter.value;
    const grade = gradeFilter.value;

    const stageNames = {
        primary: 'المرحلة الابتدائية',
        preparatory: 'المرحلة المتوسطة',
        secondary: 'المرحلة الثانوية'
    };

    const imageName = `${stageNames[stage]}_الصف_${grade}.png`;
    const imageUrl = `/images/tables/${encodeURIComponent(imageName)}`;

    const img = new Image();
    img.onload = function () {
        scheduleImage.src = imageUrl;
        scheduleImage.style.display = 'block';
        noScheduleMsg.style.display = 'none';
    };
    img.onerror = function () {
        scheduleImage.style.display = 'none';
        noScheduleMsg.style.display = 'block';
    };
    img.src = imageUrl;
}

// ================= تحديث كل شيء عند تغيير الفلاتر =================
function refreshAll() {
    updateStudentsList();
    updateScheduleImage();
}

// ================= الربط  =================
stageFilter.addEventListener('change', refreshAll);
gradeFilter.addEventListener('change', refreshAll);
subjectFilter.addEventListener('change', () => updateGradesTable());
examFilter.addEventListener('change', () => updateGradesTable());
studentGradeFilter.addEventListener('change', (e) => {
    if (e.target.value) {
        selectStudent(parseInt(e.target.value));
    }
});

refreshAll();
// ================= بيانات الطلاب والدرجات والحضور =================
const studentsData = {
    Kindergarten: {
        1: [
            { id: 1, name: "محمد محمود عبدالرحيم", grades: { math: { midterm1: 28, final1: 29, midterm2: 27, final2: 30 }, arabic: { midterm1: 27, final1: 28, midterm2: 26, final2: 29 }, english: { midterm1: 25, final1: 27, midterm2: 24, final2: 28 }, science: { midterm1: 26, final1: 28, midterm2: 25, final2: 29 } }, attendance: 94 },
            { id: 2, name: "خالد السيد محمد", grades: { math: { midterm1: 25, final1: 27, midterm2: 24, final2: 28 }, arabic: { midterm1: 24, final1: 26, midterm2: 23, final2: 27 }, english: { midterm1: 22, final1: 24, midterm2: 21, final2: 25 }, science: { midterm1: 23, final1: 25, midterm2: 22, final2: 26 } }, attendance: 85 },
            { id: 3, name: "مازن محمد عبدالعظيم", grades: { math: { midterm1: 30, final1: 30, midterm2: 29, final2: 30 }, arabic: { midterm1: 29, final1: 30, midterm2: 28, final2: 30 }, english: { midterm1: 28, final1: 29, midterm2: 27, final2: 30 }, science: { midterm1: 30, final1: 30, midterm2: 29, final2: 30 } }, attendance: 98 },
            { id: 4, name: "صالح عبدالله محمد", grades: { math: { midterm1: 26, final1: 27, midterm2: 25, final2: 28 }, arabic: { midterm1: 25, final1: 26, midterm2: 24, final2: 27 }, english: { midterm1: 23, final1: 25, midterm2: 22, final2: 26 }, science: { midterm1: 24, final1: 26, midterm2: 23, final2: 27 } }, attendance: 87 }
        ]
    },
    primary: {
        1: [
            { id: 5, name: "محمد أحمد السيد", grades: { math: { midterm1: 28, final1: 29, midterm2: 27, final2: 30 }, arabic: { midterm1: 25, final1: 28, midterm2: 26, final2: 29 }, english: { midterm1: 22, final1: 25, midterm2: 24, final2: 27 }, science: { midterm1: 26, final1: 28, midterm2: 25, final2: 29 } }, attendance: 92 },
            { id: 6, name: "فاطمة محمود حسن", grades: { math: { midterm1: 29, final1: 30, midterm2: 28, final2: 29 }, arabic: { midterm1: 28, final1: 29, midterm2: 27, final2: 30 }, english: { midterm1: 26, final1: 28, midterm2: 25, final2: 29 }, science: { midterm1: 28, final1: 29, midterm2: 27, final2: 30 } }, attendance: 95 },
            { id: 7, name: "خالد السيد محمد", grades: { math: { midterm1: 25, final1: 27, midterm2: 24, final2: 28 }, arabic: { midterm1: 24, final1: 26, midterm2: 23, final2: 27 }, english: { midterm1: 22, final1: 24, midterm2: 21, final2: 25 }, science: { midterm1: 23, final1: 25, midterm2: 22, final2: 26 } }, attendance: 85 },
            { id: 8, name: "مازن محمد عبدالعظيم", grades: { math: { midterm1: 30, final1: 30, midterm2: 29, final2: 30 }, arabic: { midterm1: 29, final1: 30, midterm2: 28, final2: 30 }, english: { midterm1: 28, final1: 29, midterm2: 27, final2: 30 }, science: { midterm1: 30, final1: 30, midterm2: 29, final2: 30 } }, attendance: 98 },
            { id: 9, name: "روان محمد ياسر", grades: { math: { midterm1: 27, final1: 28, midterm2: 26, final2: 29 }, arabic: { midterm1: 26, final1: 27, midterm2: 25, final2: 28 }, english: { midterm1: 24, final1: 26, midterm2: 23, final2: 27 }, science: { midterm1: 25, final1: 27, midterm2: 24, final2: 28 } }, attendance: 90 }
        ],
        2: [
            { id: 10, name: "فيروز أحمد محمد", grades: { math: { midterm1: 26, final1: 28, midterm2: 25, final2: 29 }, arabic: { midterm1: 25, final1: 27, midterm2: 24, final2: 28 }, english: { midterm1: 23, final1: 25, midterm2: 22, final2: 26 }, science: { midterm1: 24, final1: 26, midterm2: 23, final2: 27 } }, attendance: 88 },
            { id: 11, name: "آمنة محمد عبدالله", grades: { math: { midterm1: 29, final1: 29, midterm2: 28, final2: 30 }, arabic: { midterm1: 28, final1: 29, midterm2: 27, final2: 29 }, english: { midterm1: 26, final1: 28, midterm2: 25, final2: 29 }, science: { midterm1: 27, final1: 29, midterm2: 26, final2: 30 } }, attendance: 93 },
            { id: 12, name: "فريدة أحمد صالح", grades: { math: { midterm1: 27, final1: 28, midterm2: 26, final2: 29 }, arabic: { midterm1: 26, final1: 28, midterm2: 25, final2: 28 }, english: { midterm1: 24, final1: 26, midterm2: 23, final2: 27 }, science: { midterm1: 25, final1: 27, midterm2: 24, final2: 28 } }, attendance: 91 }
        ]
    },
    preparatory: {
        1: [
            { id: 13, name: "محمد محمود عبدالرحيم", grades: { math: { midterm1: 28, final1: 29, midterm2: 27, final2: 30 }, arabic: { midterm1: 27, final1: 28, midterm2: 26, final2: 29 }, english: { midterm1: 25, final1: 27, midterm2: 24, final2: 28 }, science: { midterm1: 26, final1: 28, midterm2: 25, final2: 29 } }, attendance: 94 },
            { id: 14, name: "صالح عبدالله محمد", grades: { math: { midterm1: 26, final1: 27, midterm2: 25, final2: 28 }, arabic: { midterm1: 25, final1: 26, midterm2: 24, final2: 27 }, english: { midterm1: 23, final1: 25, midterm2: 22, final2: 26 }, science: { midterm1: 24, final1: 26, midterm2: 23, final2: 27 } }, attendance: 87 }
        ]
    },
    secondary: {
        1: [
            { id: 15, name: "ابراهيم أحمد محمد", grades: { math: { midterm1: 26, final1: 28, midterm2: 25, final2: 29 }, arabic: { midterm1: 25, final1: 27, midterm2: 24, final2: 28 }, english: { midterm1: 23, final1: 25, midterm2: 22, final2: 26 }, science: { midterm1: 24, final1: 26, midterm2: 23, final2: 27 } }, attendance: 88 },
            { id: 16, name: "ايمان محمد عبدالله", grades: { math: { midterm1: 29, final1: 29, midterm2: 28, final2: 30 }, arabic: { midterm1: 28, final1: 29, midterm2: 27, final2: 29 }, english: { midterm1: 26, final1: 28, midterm2: 25, final2: 29 }, science: { midterm1: 27, final1: 29, midterm2: 26, final2: 30 } }, attendance: 93 },
            { id: 17, name: "محمد أحمد صالح", grades: { math: { midterm1: 27, final1: 28, midterm2: 26, final2: 29 }, arabic: { midterm1: 26, final1: 28, midterm2: 25, final2: 28 }, english: { midterm1: 24, final1: 26, midterm2: 23, final2: 27 }, science: { midterm1: 25, final1: 27, midterm2: 24, final2: 28 } }, attendance: 81 }
        ]
    }
};

// ================= العناصر المربوطة بـ DOM =================
const stageFilter = document.getElementById('stageFilter');
const gradeFilter = document.getElementById('gradeFilter');
const studentsList = document.getElementById('studentsList');
const scheduleImage = document.getElementById('scheduleImage');
const noScheduleMsg = document.getElementById('noScheduleMsg');
const studentGradeFilter = document.getElementById('studentGradeFilter');
const subjectFilter = document.getElementById('subjectFilter');
const examFilter = document.getElementById('examFilter');
const gradesTableBody = document.getElementById('gradesTableBody');

let attendanceChart = null;
let currentStudents = [];
let currentSelectedStudent = null;

const examMaxScores = { midterm1: 30, final1: 30, midterm2: 30, final2: 30 };

// ================= تحديث قائمة الطلاب والفلاتر =================
function updateStudentsList() {
    const stage = stageFilter.value;
    const grade = gradeFilter.value;

    currentStudents = studentsData[stage]?.[grade] || [];

    studentsList.innerHTML = '';
    studentGradeFilter.innerHTML = '<option value="">اختر الطالب</option>';

    if (currentStudents.length === 0) {
        studentsList.innerHTML = '<li style="text-align:center; color:#888;">لا يوجد طلاب في هذا الصف</li>';
        gradesTableBody.innerHTML = '<tr><td colspan="6" style="text-align:center;">لا توجد بيانات لعرضها</td></tr>';
        currentSelectedStudent = null;
        if (attendanceChart) attendanceChart.destroy();
        return;
    }

    currentStudents.forEach((student) => {
        // إنشاء عنصر القائمة الجانبية
        const li = document.createElement('li');
        li.textContent = student.name;
        li.dataset.id = student.id;
        li.onclick = () => selectStudent(student.id);
        studentsList.appendChild(li);

        // إضافة الطالب لخيارات فلتر الدرجات
        const option = document.createElement('option');
        option.value = student.id;
        option.textContent = student.name;
        studentGradeFilter.appendChild(option);
    });

    // تحديد أول طالب تلقائياً عند تحميل القائمة
    if (currentStudents.length > 0) {
        selectStudent(currentStudents[0].id);
    }
}

// ================= اختيار طالب محدد =================
function selectStudent(studentId) {
    currentSelectedStudent = currentStudents.find(s => s.id == studentId);
    if (!currentSelectedStudent) return;

    // إضافة الكلاس النشط (active) للعنصر المختار في القائمة
    document.querySelectorAll('.students-list li').forEach(li => {
        li.classList.remove('active');
        if (li.dataset.id == studentId) {
            li.classList.add('active');
        }
    });

    // مزامنة فلتر الـ Select الخاص بالدرجات
    studentGradeFilter.value = studentId;

    // تحديث الجدول والرسم البياني
    updateGradesTable();
    updateAttendanceChart();
}

// ================= تحديث جدول الدرجات للصف بالكامل =================
function updateGradesTable() {
    if (currentStudents.length === 0) return;

    const subject = subjectFilter.value;
    const exam = examFilter.value;
    const maxScore = examMaxScores[exam];

    gradesTableBody.innerHTML = '';

    currentStudents.forEach((student, index) => {
        const studentGrade = student.grades[subject]?.[exam] ?? 0;
        const studentPercentage = ((studentGrade / maxScore) * 100).toFixed(1);
        const studentPassed = studentGrade >= (maxScore * 0.5);

        const row = gradesTableBody.insertRow();
        row.innerHTML = `
            <td>${index + 1}</td>
            <td>${student.name}</td>
            <td>${studentGrade}</td>
            <td>${maxScore}</td>
            <td>${studentPercentage}%</td>
            <td class="${studentPassed ? 'status-pass' : 'status-fail'}">${studentPassed ? 'ناجح' : 'راسب'}</td>
        `;

        // تمييز السطر الخاص بالطالب المحدد حالياً بلون خلفية مختلف
        if (currentSelectedStudent && student.id == currentSelectedStudent.id) {
            row.style.background = '#eef3f6';
            row.style.fontWeight = 'bold';
        }
    });
}

// ================= تحديث الرسم البياني للحضور والغياب (Chart.js) =================
function updateAttendanceChart() {
    if (!currentSelectedStudent) {
        if (attendanceChart) attendanceChart.destroy();
        return;
    }

    const attendance = currentSelectedStudent.attendance || 0;
    const absence = 100 - attendance;

    if (attendanceChart) {
        attendanceChart.destroy();
    }

    const ctx = document.getElementById('attendanceChart').getContext('2d');
    attendanceChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ['حضور', 'غياب'],
            datasets: [{
                data: [attendance, absence],
                backgroundColor: ['#2ecc71', '#e74c3c'],
                borderWidth: 0
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: true,
            plugins: {
                legend: {
                    position: 'bottom',
                    labels: { font: { size: 12 } }
                },
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            return `${context.label}: ${context.raw}%`;
                        }
                    }
                }
            }
        }
    });
}

// ================= تحديث صورة الجدول الدراسي =================
function updateScheduleImage() {
    const stage = stageFilter.value;
    const grade = gradeFilter.value;

    const stageNames = {
        Kindergarten: 'رياض الاطفال',
        primary: 'المرحلة الابتدائية',
        preparatory: 'المرحلة المتوسطة',
        secondary: 'المرحلة الثانوية'
    };

    const imageName = `${stageNames[stage]}_الصف_${grade}.png`;
    const imageUrl = `/images/tables/${encodeURIComponent(imageName)}`;

    const img = new Image();
    img.onload = function () {
        scheduleImage.src = imageUrl;
        scheduleImage.style.display = 'block';
        noScheduleMsg.style.display = 'none';
    };
    img.onerror = function () {
        scheduleImage.style.display = 'none';
        noScheduleMsg.style.display = 'block';
    };
    img.src = imageUrl;
}

// ================= دالة التحديث الشاملة =================
function refreshAll() {
    updateStudentsList();
    updateScheduleImage();
}

// ================= ربط الأحداث (Event Listeners) =================
stageFilter.addEventListener('change', refreshAll);
gradeFilter.addEventListener('change', refreshAll);
subjectFilter.addEventListener('change', updateGradesTable);
examFilter.addEventListener('change', updateGradesTable);

studentGradeFilter.addEventListener('change', (e) => {
    if (e.target.value) {
        selectStudent(parseInt(e.target.value));
    }
});

// التشغيل المبدئي عند تحميل الصفحة
refreshAll();