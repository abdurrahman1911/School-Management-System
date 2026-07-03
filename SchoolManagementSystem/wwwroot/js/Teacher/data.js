
// Test data
const exams = {
    '1-primary': [
        { 
            id: 1, 
            name: "اختبار الرياضيات الفصل الأول", 
            subject: "math", 
            type: "final",
            date: "20-10-2025",
            time: "08:00",
            duration: 45,
            totalQuestions: 10,
            totalMarks: 100,
            passingMarks: 50,
            status: "upcoming",
            questions: [
                {
                    id: 1,
                    question: "ما هو ناتج جمع 5 + 3؟",
                    options: [
                        { id: 'a', text: "7", isCorrect: false },
                        { id: 'b', text: "8", isCorrect: true },
                        { id: 'c', text: "9", isCorrect: false },
                        { id: 'd', text: "10", isCorrect: false }
                    ],
                    marks: 10
                },
                {
                    id: 2,
                    question: "ما هو ناتج طرح 10 - 4؟",
                    options: [
                        { id: 'a', text: "5", isCorrect: false },
                        { id: 'b', text: "6", isCorrect: true },
                        { id: 'c', text: "7", isCorrect: false },
                        { id: 'd', text: "8", isCorrect: false }
                    ],
                    marks: 10
                }
            ],
            notes: "اختبار شامل لطلاب الصف الأول",
            studentsSubmitted: [1001, 1002],
            grades: {
                '1001': 85,
                '1002': 70
            }
        }
    ],
    
    '6-primary': [
        { 
            id: 2, 
            name: "اختبار العلوم نصف الفصل", 
            subject: "science", 
            type: "midterm",
            date: "25-10-2025",
            time: "09:30",
            duration: 60,
            totalQuestions: 15,
            totalMarks: 100,
            passingMarks: 50,
            status: "upcoming",
            questions: [
                {
                    id: 1,
                    question: "ما هو أكبر كوكب في المجموعة الشمسية؟",
                    options: [
                        { id: 'a', text: "الأرض", isCorrect: false },
                        { id: 'b', text: "المشتري", isCorrect: true },
                        { id: 'c', text: "زحل", isCorrect: false },
                        { id: 'd', text: "المريخ", isCorrect: false }
                    ],
                    marks: 10
                }
            ],
            notes: "اختبار منتصف الفصل الدراسي",
            studentsSubmitted: [6001, 6002],
            grades: {
                '6001': 92,
                '6002': 88
            }
        }
    ],
    
    '1-preparatory': [
        { 
            id: 3, 
            name: "اختبار اللغة الإنجليزية النهائي", 
            subject: "english", 
            type: "final",
            date: "30-10-2025",
            time: "10:00",
            duration: 90,
            totalQuestions: 20,
            totalMarks: 100,
            passingMarks: 50,
            status: "upcoming",
            questions: [
                {
                    id: 1,
                    question: "What is the capital of France?",
                    options: [
                        { id: 'a', text: "London", isCorrect: false },
                        { id: 'b', text: "Berlin", isCorrect: false },
                        { id: 'c', text: "Paris", isCorrect: true },
                        { id: 'd', text: "Madrid", isCorrect: false }
                    ],
                    marks: 10
                }
            ],
            notes: "Final exam for first preparatory",
            studentsSubmitted: [7101],
            grades: {
                '7101': 78
            }
        },
        { 
            id: 5, 
            name: "اختبار الرياضيات الأول", 
            subject: "math", 
            type: "quiz",
            date: "15-10-2025",
            time: "08:30",
            duration: 30,
            totalQuestions: 5,
            totalMarks: 50,
            passingMarks: 25,
            status: "completed",
            questions: [
                {
                    id: 1,
                    question: "ما هو ناتج 12 × 3؟",
                    options: [
                        { id: 'a', text: "34", isCorrect: false },
                        { id: 'b', text: "36", isCorrect: true },
                        { id: 'c', text: "38", isCorrect: false },
                        { id: 'd', text: "40", isCorrect: false }
                    ],
                    marks: 10
                }
            ],
            notes: "اختبار قصير",
            studentsSubmitted: [7101, 7102],
            grades: {
                '7101': 45,
                '7102': 40
            }
        }
    ],
    
    '1-secondary': [
        { 
            id: 4, 
            name: "اختبار الكيمياء العملي", 
            subject: "science", 
            type: "practical",
            date: "15-11-2025",
            time: "11:00",
            duration: 120,
            totalQuestions: 8,
            totalMarks: 80,
            passingMarks: 40,
            status: "upcoming",
            questions: [
                {
                    id: 1,
                    question: "ما هي الصيغة الكيميائية للماء؟",
                    options: [
                        { id: 'a', text: "CO2", isCorrect: false },
                        { id: 'b', text: "H2O", isCorrect: true },
                        { id: 'c', text: "NaCl", isCorrect: false },
                        { id: 'd', text: "O2", isCorrect: false }
                    ],
                    marks: 10
                }
            ],
            notes: "اختبار عملي في المختبر",
            studentsSubmitted: [10101],
            grades: {
                '10101': 75
            }
        }
    ],
    
    '2-primary': [
        { 
            id: 6, 
            name: "اختبار اللغة العربية", 
            subject: "arabic", 
            type: "quiz",
            date: "18-10-2025",
            time: "09:00",
            duration: 40,
            totalQuestions: 8,
            totalMarks: 80,
            passingMarks: 40,
            status: "ongoing",
            questions: [
                {
                    id: 1,
                    question: "ما هو جمع كلمة 'كتاب'؟",
                    options: [
                        { id: 'a', text: "كتب", isCorrect: true },
                        { id: 'b', text: "كتابات", isCorrect: false },
                        { id: 'c', text: "كتيب", isCorrect: false },
                        { id: 'd', text: "مكتبة", isCorrect: false }
                    ],
                    marks: 10
                }
            ],
            notes: "اختبار قصير في اللغة العربية",
            studentsSubmitted: [2001],
            grades: {
                '2001': 85
            }
        }
    ]
};

// Student data
const students = {
    '1001': { name: "علي محمد أحمد", grade: "1-primary" },
    '1002': { name: "سارة خالد حسن", grade: "1-primary" },
    '1003': { name: "محمد علي أحمد", grade: "1-primary" },
    '1004': { name: "فاطمة يوسف محمد", grade: "1-primary" },
    '2001': { name: "يوسف أحمد علي", grade: "2-primary" },
    '6001': { name: "محمد أحمد السيد", grade: "6-primary" },
    '6002': { name: "فاطمة محمود حسن", grade: "6-primary" },
    '6003': { name: "خالد السيد محمد", grade: "6-primary" },
    '7101': { name: "أحمد علي محمود", grade: "1-preparatory" },
    '7102': { name: "سارة محمد حسين", grade: "1-preparatory" },
    '10101': { name: "يوسف عبدالله كريم", grade: "1-secondary" }
};

const assignments = {
    '1-primary': [
        { 
            id: 1, 
            name: "تعلم الحروف", 
            subject: "arabic", 
            startDate: "15-10-2025", 
            endDate: "18-10-2025"
        }
    ],
    '6-primary': [
        { 
            id: 2, 
            name: "تمارين الرياضيات الفصل 3", 
            subject: "math", 
            startDate: "16-10-2025", 
            endDate: "18-10-2025"
        }
    ],
    '1-preparatory': [
        { 
            id: 3, 
            name: "بحث عن النظام الشمسي", 
            subject: "science", 
            startDate: "18-10-2025", 
            endDate: "20-10-2025"
        }
    ]
};


function formatDateForDisplay(dateString) {
    if (!dateString || dateString === 'غير محدد') return 'غير محدد';
    return dateString;
}

function getDateInputFormat(dateString) {
    if (!dateString || dateString === 'غير محدد') return '';
    const parts = dateString.split('-');
    if (parts.length !== 3) return '';
    return `${parts[2]}-${parts[1]}-${parts[0]}`;
}

function getSubjectName(subject) {
    const subjects = {
        "math": "الرياضيات",
        "arabic": "اللغة العربية",
        "english": "اللغة الإنجليزية",
        "science": "العلوم",
        "social": "الدراسات الاجتماعية"
    };
    return subjects[subject] || subject;
}

function getExamTypeText(type) {
    const types = {
        "quiz": "اختبار قصير",
        "midterm": "منتصف الفصل",
        "final": "نهائي",
        "practical": "عملي"
    };
    return types[type] || type;
}

function getExamStatusText(status) {
    const statuses = {
        "upcoming": "قادم",
        "ongoing": "جاري",
        "completed": "منتهي",
        "graded": "مصحح"
    };
    return statuses[status] || status;
}

function getExamStatusClass(status) {
    const classes = {
        "upcoming": "status-upcoming",
        "ongoing": "status-ongoing",
        "completed": "status-completed",
        "graded": "status-completed"
    };
    return classes[status] || "";
}

function getLevelDisplayName(level) {
    const levels = {
        "primary": "المرحلة الابتدائية",
        "preparatory": "المرحلة المتوسطة",
        "secondary": "المرحلة الثانوية"
    };
    return levels[level] || level;
}

function getGradeDisplayInfo(gradeKey) {
    const gradeNames = {
        "1-primary": "الصف الأول الابتدائي",
        "2-primary": "الصف الثاني الابتدائي",
        "3-primary": "الصف الثالث الابتدائي",
        "4-primary": "الصف الرابع الابتدائي",
        "5-primary": "الصف الخامس الابتدائي",
        "6-primary": "الصف السادس الابتدائي",
        "1-preparatory": "الصف الأول الإعدادي",
        "2-preparatory": "الصف الثاني الإعدادي",
        "3-preparatory": "الصف الثالث الإعدادي",
        "1-secondary": "الصف الأول الثانوي",
        "2-secondary": "الصف الثاني الثانوي",
        "3-secondary": "الصف الثالث الثانوي"
    };
    return gradeNames[gradeKey] || gradeKey;
}

function getStudentName(studentId) {
    return students[studentId]?.name || `طالب ${studentId}`;
}

function getStudentGrade(studentId) {
    return students[studentId]?.grade || '';
}

// Function to update statistics
function updateExamStatistics() {
    const allExams = [];
    Object.values(exams).forEach(examList => {
        allExams.push(...examList);
    });

    const stats = {
        total: allExams.length,
        upcoming: allExams.filter(e => e.status === 'upcoming').length,
        ongoing: allExams.filter(e => e.status === 'ongoing').length,
        completed: allExams.filter(e => e.status === 'completed').length,
        graded: allExams.filter(e => e.grades && Object.keys(e.grades).length > 0).length
    };

    return stats;
}


// function to obtain scores for a specific test
function getExamGrades(examId) {
    for (const gradeKey in exams) {
        const exam = exams[gradeKey].find(e => e.id === examId);
        if (exam && exam.grades) {
            return exam.grades;
        }
    }
    return {};
}


//  function to retrieve information for a specific test
function getExamInfo(examId) {
    for (const gradeKey in exams) {
        const exam = exams[gradeKey].find(e => e.id === examId);
        if (exam) {
            return { exam, gradeKey };
        }
    }
    return { exam: null, gradeKey: '' };
}



// Function to add a new test

function addNewExam(examData, gradeKey) {
    if (!exams[gradeKey]) {
        exams[gradeKey] = [];
    }
    
// Generate a new ID
    const newId = Math.max(...Object.values(exams).flat().map(e => e.id), 0) + 1;
    examData.id = newId;
    
    exams[gradeKey].push(examData);
    return newId;
}

// Function to update an existing test
function updateExam(examId, gradeKey, updatedData) {
    const gradeExams = exams[gradeKey];
    if (!gradeExams) return false;
    
    const examIndex = gradeExams.findIndex(e => e.id === examId);
    if (examIndex === -1) return false;
    
    exams[gradeKey][examIndex] = { ...exams[gradeKey][examIndex], ...updatedData };
    return true;
}

// Function to delete a test

function deleteExam(examId) {
    for (const gradeKey in exams) {
        const examIndex = exams[gradeKey].findIndex(e => e.id === examId);
        if (examIndex !== -1) {
            exams[gradeKey].splice(examIndex, 1);
            return true;
        }
    }
    return false;
}

// Function to add a student's score on a test

function addExamGrade(examId, studentId, grade) {
    const examInfo = getExamInfo(examId);
    if (!examInfo.exam) return false;
    
    if (!examInfo.exam.grades) {
        examInfo.exam.grades = {};
    }
    
    examInfo.exam.grades[studentId] = grade;
    return true;
}

function getAllGrades() {
    return [
        { value: '1-primary', name: 'الصف الأول الابتدائي' },
        { value: '2-primary', name: 'الصف الثاني الابتدائي' },
        { value: '3-primary', name: 'الصف الثالث الابتدائي' },
        { value: '4-primary', name: 'الصف الرابع الابتدائي' },
        { value: '5-primary', name: 'الصف الخامس الابتدائي' },
        { value: '6-primary', name: 'الصف السادس الابتدائي' },
        { value: '1-preparatory', name: 'الصف الأول الإعدادي' },
        { value: '2-preparatory', name: 'الصف الثاني الإعدادي' },
        { value: '3-preparatory', name: 'الصف الثالث الإعدادي' },
        { value: '1-secondary', name: 'الصف الأول الثانوي' },
        { value: '2-secondary', name: 'الصف الثاني الثانوي' },
        { value: '3-secondary', name: 'الصف الثالث الثانوي' }
    ];
}


function getGradesByLevel(level) {
    const gradeList = [];
    
    if (level === 'all' || level === 'primary') {
        gradeList.push(
            { value: '1-primary', name: 'الصف الأول الابتدائي' },
            { value: '2-primary', name: 'الصف الثاني الابتدائي' },
            { value: '3-primary', name: 'الصف الثالث الابتدائي' },
            { value: '4-primary', name: 'الصف الرابع الابتدائي' },
            { value: '5-primary', name: 'الصف الخامس الابتدائي' },
            { value: '6-primary', name: 'الصف السادس الابتدائي' }
        );
    }
    
    if (level === 'all' || level === 'preparatory') {
        gradeList.push(
            { value: '1-preparatory', name: 'الصف الأول الإعدادي' },
            { value: '2-preparatory', name: 'الصف الثاني الإعدادي' },
            { value: '3-preparatory', name: 'الصف الثالث الإعدادي' }
        );
    }
    
    if (level === 'all' || level === 'secondary') {
        gradeList.push(
            { value: '1-secondary', name: 'الصف الأول الثانوي' },
            { value: '2-secondary', name: 'الصف الثاني الثانوي' },
            { value: '3-secondary', name: 'الصف الثالث الثانوي' }
        );
    }
    
    return gradeList;
}

function showNotification(message, type = 'info') {

    const existingNotification = document.querySelector('.notification');
    if (existingNotification) {
        existingNotification.remove();
    }
    
    const notification = document.createElement('div');
    notification.className = `notification ${type}`;
    
    const icon = type === 'success' ? 'fa-check-circle' : 
                 type === 'error' ? 'fa-exclamation-circle' : 'fa-info-circle';
                 
    notification.innerHTML = `<i class="fas ${icon}"></i> ${message}`;
    document.body.appendChild(notification);
    
    setTimeout(() => {
        if (notification.parentNode) {
            notification.remove();
        }
    }, 3000);
}

function initializeAttendanceData() {
    const today = getTodayDate();
    
    Object.keys(students).forEach(gradeKey => {
        if (!attendanceData[gradeKey]) {
            attendanceData[gradeKey] = {};
        }
        
        const gradeStudents = students[gradeKey];
        if (gradeStudents && gradeStudents.length > 0) {
            if (!attendanceData[gradeKey][today]) {
                attendanceData[gradeKey][today] = {};
            }
            
            gradeStudents.forEach(student => {
                if (!attendanceData[gradeKey][today][student.id]) {
                    attendanceData[gradeKey][today][student.id] = 'present';
                }
            });
        }
    });
}

function calculateAverageGrade(grades) {
    if (!grades || Object.keys(grades).length === 0) return 0;
    
    const sum = Object.values(grades).reduce((total, grade) => total + grade, 0);
    return Math.round(sum / Object.keys(grades).length);
}


// Checking student success

function isStudentPassed(grade, passingMarks) {
    return grade >= passingMarks;
}


// Student performance analysis

function analyzeExamPerformance(examId) {
    const examInfo = getExamInfo(examId);
    if (!examInfo.exam || !examInfo.exam.grades) {
        return null;
    }
    
    const grades = examInfo.exam.grades;
    const passingMarks = examInfo.exam.passingMarks || 50;
    
    const result = {
        totalStudents: Object.keys(grades).length,
        average: calculateAverageGrade(grades),
        passed: 0,
        failed: 0,
        highest: Math.max(...Object.values(grades)),
        lowest: Math.min(...Object.values(grades))
    };
    
    Object.values(grades).forEach(grade => {
        if (grade >= passingMarks) {
            result.passed++;
        } else {
            result.failed++;
        }
    });
    
    result.passPercentage = Math.round((result.passed / result.totalStudents) * 100);
    
    return result;
}