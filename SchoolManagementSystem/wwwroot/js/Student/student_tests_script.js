
const testsData = [
    {
        id: 1,
        subject: "الكيمياء",
        subjectCode: "CHEM-301",
        title: "التوزيع الإلكتروني والجدول الدوري",
        type: "اختبار شهري",
        date: "2026-01-20",
        time: "09:00",
        dueTime: "10:30 ص",
        status: "upcoming",
        duration: "90 دقيقة",
        totalMarks: 40,
        chapter: "الفصل الثالث",
        instructions: [
            "يمنع استخدام الآلة الحاسبة",
            "الإجابة بالقلم الأزرق فقط",
            "لا يسمح بالخروج قبل انتهاء نصف الوقت"
        ]
    },
    {
        id: 2,
        subject: "الفيزياء",
        subjectCode: "PHYS-301",
        title: "الحركة التوافقية البسيطة",
        type: "اختبار نهائي",
        date: "2026-01-25",
        time: "08:00",
        dueTime: "10:00 ص",
        status: "upcoming",
        duration: "120 دقيقة",
        totalMarks: 60,
        chapter: "الفصل الرابع والخامس",
        instructions: [
            "يسمح باستخدام الآلة الحاسبة العلمية",
            "الجداول والقوانين مرفقة مع الاختبار",
            "اكتب جميع خطوات الحل"
        ]
    },
    {
        id: 3,
        subject: "الرياضيات",
        subjectCode: "MATH-301",
        title: "التفاضل والتكامل - اختبار قصير",
        type: "اختبار قصير",
        date: new Date().toISOString().split('T')[0],
        time: new Date().getHours() + ":" + (new Date().getMinutes() < 10 ? '0' : '') + new Date().getMinutes(),
        dueTime: "11:59 م",
        status: "active",
        duration: "30 دقيقة",
        totalMarks: 20,
        chapter: "الفصل الثاني",
        instructions: [
            "اختبار إلكتروني",
            "يتم التسليم التلقائي عند انتهاء الوقت",
            "لا يمكن العودة للسؤال السابق"
        ]
    },
    {
        id: 4,
        subject: "اللغة العربية",
        subjectCode: "ARAB-301",
        title: "النحو والصرف - الإعراب",
        type: "اختبار شهري",
        date: "2026-01-10",
        time: "10:00",
        dueTime: "11:00 ص",
        status: "completed",
        duration: "60 دقيقة",
        totalMarks: 30,
        grade: 27,
        percentage: 90,
        submittedDate: "2026-01-10 10:55 ص",
        chapter: "الفصل الأول والثاني",
        feedback: "أداء ممتاز، يحتاج تحسين في باب الإعراب"
    },
    {
        id: 5,
        subject: "اللغة الإنجليزية",
        subjectCode: "ENG-301",
        title: "Grammar and Vocabulary Test",
        type: "اختبار شهري",
        date: "2026-01-08",
        time: "09:00",
        dueTime: "10:00 ص",
        status: "completed",
        duration: "60 دقيقة",
        totalMarks: 30,
        grade: 25,
        percentage: 83,
        submittedDate: "2026-01-08 09:50 ص",
        chapter: "Units 3-4"
    },
    {
        id: 6,
        subject: "الأحياء",
        subjectCode: "BIO-301",
        title: "الخلية والأنسجة النباتية",
        type: "اختبار قصير",
        date: "2026-01-05",
        time: "11:00",
        dueTime: "11:30 ص",
        status: "missed",
        duration: "30 دقيقة",
        totalMarks: 15,
        chapter: "الفصل الأول",
        missedReason: "غياب بعذر طبي"
    },
    {
        id: 7,
        subject: "التاريخ",
        subjectCode: "HIST-301",
        title: "الحضارات القديمة",
        type: "اختبار شهري",
        date: "2026-01-12",
        time: "08:00",
        dueTime: "09:00 ص",
        status: "completed",
        duration: "60 دقيقة",
        totalMarks: 25,
        grade: 22,
        percentage: 88,
        submittedDate: "2026-01-12 08:55 ص",
        chapter: "الوحدة الأولى"
    },
    {
        id: 8,
        subject: "الجغرافيا",
        subjectCode: "GEO-301",
        title: "المناخ والطقس",
        type: "اختبار عملي",
        date: "2026-01-22",
        time: "10:00",
        dueTime: "11:30 ص",
        status: "upcoming",
        duration: "90 دقيقة",
        totalMarks: 30,
        chapter: "الفصل الثاني",
        instructions: [
            "اختبار عملي يتضمن قراءة الخرائط",
            "إحضار أدوات الرسم الهندسي",
            "يسمح باستخدام الأطلس"
        ]
    }
];

// DOM Elements
let testsContainer, tabs, subjectFilter, searchInput, emptyState;
let nextTestBanner, countdownInterval;

document.addEventListener('DOMContentLoaded', function () {
    // Initialize DOM references
    testsContainer = document.getElementById('tests-container');
    tabs = document.querySelectorAll('.tests-tab-btn');
    subjectFilter = document.getElementById('subject-filter');
    searchInput = document.getElementById('tests-search');
    emptyState = document.getElementById('empty-state');
    nextTestBanner = document.getElementById('next-test-banner');

    // Initialize the page
    initializePage();
});

function initializePage() {
    // Update statistics
    updateStatistics();

    // Populate subject filter
    populateSubjectFilter();

    // Setup next test countdown
    setupNextTestCountdown();

    // Initial render
    renderTests('all');

    // Setup event listeners
    setupEventListeners();

    // Setup modal
    setupModal();
}

function updateStatistics() {
    const upcoming = testsData.filter(t => t.status === 'upcoming').length;
    const active = testsData.filter(t => t.status === 'active').length;
    const completed = testsData.filter(t => t.status === 'completed').length;
    const missed = testsData.filter(t => t.status === 'missed').length;

    document.getElementById('upcoming-count').textContent = upcoming;
    document.getElementById('active-count').textContent = active;
    document.getElementById('completed-count').textContent = completed;
    document.getElementById('missed-count').textContent = missed;
}

function populateSubjectFilter() {
    const subjects = [...new Set(testsData.map(t => t.subject))];
    subjects.forEach(subject => {
        const option = document.createElement('option');
        option.value = subject;
        option.textContent = subject;
        subjectFilter.appendChild(option);
    });
}

function setupNextTestCountdown() {
    // Find the next upcoming test
    const upcomingTests = testsData
        .filter(t => t.status === 'upcoming' || t.status === 'active')
        .sort((a, b) => new Date(a.date) - new Date(b.date));

    if (upcomingTests.length > 0) {
        const nextTest = upcomingTests[0];
        nextTestBanner.style.display = 'flex';
        document.getElementById('next-test-title').textContent = nextTest.title;
        document.getElementById('next-test-subject').textContent = nextTest.subject + ' - ' + nextTest.type;

        // Start countdown
        updateCountdown(nextTest);
        countdownInterval = setInterval(() => updateCountdown(nextTest), 60000);
    }
}

function updateCountdown(test) {
    const testDate = new Date(test.date + 'T' + (test.time || '08:00'));
    const now = new Date();
    const diff = testDate - now;

    if (diff <= 0) {
        document.getElementById('countdown-days').textContent = '00';
        document.getElementById('countdown-hours').textContent = '00';
        document.getElementById('countdown-minutes').textContent = '00';
        return;
    }

    const days = Math.floor(diff / (1000 * 60 * 60 * 24));
    const hours = Math.floor((diff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    const minutes = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));

    document.getElementById('countdown-days').textContent = String(days).padStart(2, '0');
    document.getElementById('countdown-hours').textContent = String(hours).padStart(2, '0');
    document.getElementById('countdown-minutes').textContent = String(minutes).padStart(2, '0');
}

function setupEventListeners() {
    // Tab switching
    tabs.forEach(tab => {
        tab.addEventListener('click', () => {
            tabs.forEach(t => t.classList.remove('active'));
            tab.classList.add('active');
            applyFilters();
        });
    });

    // Subject filter
    subjectFilter.addEventListener('change', applyFilters);

    // Search
    searchInput.addEventListener('input', debounce(applyFilters, 300));
}

function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

function applyFilters() {
    const activeTab = document.querySelector('.tests-tab-btn.active');
    const filter = activeTab.dataset.filter;
    const subject = subjectFilter.value;
    const searchTerm = searchInput.value.toLowerCase().trim();

    renderTests(filter, subject, searchTerm);
}

function renderTests(filter, subject = 'all', searchTerm = '') {
    testsContainer.innerHTML = '';

    let filteredTests = [...testsData];

    // Apply status filter
    if (filter === 'current') {
        filteredTests = filteredTests.filter(t => t.status === 'active' || t.status === 'upcoming');
    } else if (filter === 'completed') {
        filteredTests = filteredTests.filter(t => t.status === 'completed');
    } else if (filter === 'missed') {
        filteredTests = filteredTests.filter(t => t.status === 'missed');
    }

    // Apply subject filter
    if (subject !== 'all') {
        filteredTests = filteredTests.filter(t => t.subject === subject);
    }

    // Apply search filter
    if (searchTerm) {
        filteredTests = filteredTests.filter(t =>
            t.title.toLowerCase().includes(searchTerm) ||
            t.subject.toLowerCase().includes(searchTerm) ||
            t.type.toLowerCase().includes(searchTerm)
        );
    }

    // Sort: active first, then by date
    filteredTests.sort((a, b) => {
        if (a.status === 'active' && b.status !== 'active') return -1;
        if (b.status === 'active' && a.status !== 'active') return 1;
        return new Date(a.date) - new Date(b.date);
    });

    if (filteredTests.length === 0) {
        emptyState.style.display = 'block';
        testsContainer.style.display = 'none';
        return;
    }

    emptyState.style.display = 'none';
    testsContainer.style.display = 'grid';

    filteredTests.forEach(test => {
        const card = createTestCard(test);
        testsContainer.appendChild(card);
    });
}

function createTestCard(test) {
    const div = document.createElement('div');
    div.className = `test-card-new ${test.status}`;
    div.onclick = () => openTestModal(test);

    // Status configuration
    const statusConfig = {
        active: { badge: 'متاح الآن', icon: '🟢', class: 'active-badge' },
        upcoming: { badge: 'قادم', icon: '📅', class: 'upcoming-badge' },
        completed: { badge: 'مكتمل', icon: '✅', class: 'completed-badge' },
        missed: { badge: 'فائت', icon: '❌', class: 'missed-badge' }
    };

    const config = statusConfig[test.status];

    // Format date
    const formattedDate = formatArabicDate(test.date);

    // Build grade section for completed tests
    let gradeSection = '';
    if (test.status === 'completed' && test.grade !== undefined) {
        const gradeClass = test.percentage >= 60 ? 'grade-pass' : 'grade-fail';
        gradeSection = `
            <div class="test-grade-display ${gradeClass}">
                <span class="grade-value">${test.grade}/${test.totalMarks}</span>
                <span class="grade-percentage">${test.percentage}%</span>
            </div>
        `;
    }

    // Build action button
    let actionButton = '';
    if (test.status === 'active') {
        actionButton = `<button class="test-action-btn start-btn" onclick="event.stopPropagation(); startTest(${test.id})">بدء الاختبار</button>`;
    } else if (test.status === 'upcoming') {
        actionButton = `<button class="test-action-btn upcoming-action" disabled>يبدأ في ${formattedDate}</button>`;
    } else if (test.status === 'completed') {
        actionButton = `<button class="test-action-btn result-btn" onclick="event.stopPropagation(); viewResults(${test.id})">عرض النتيجة</button>`;
    } else if (test.status === 'missed') {
        actionButton = `<button class="test-action-btn missed-action" onclick="event.stopPropagation(); contactTeacher(${test.id})">تواصل مع المعلم</button>`;
    }

    div.innerHTML = `
        <div class="test-card-header">
            <span class="test-status-badge ${config.class}">${config.icon} ${config.badge}</span>
            <span class="test-subject-badge">${test.subject}</span>
        </div>
        <div class="test-card-body">
            <h3 class="test-card-title">${test.title}</h3>
            <div class="test-card-meta">
                <div class="meta-item">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                        <rect x="3" y="4" width="18" height="18" rx="2" ry="2"></rect>
                        <line x1="16" y1="2" x2="16" y2="6"></line>
                        <line x1="8" y1="2" x2="8" y2="6"></line>
                        <line x1="3" y1="10" x2="21" y2="10"></line>
                    </svg>
                    <span>${formattedDate}</span>
                </div>
                <div class="meta-item">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                        <circle cx="12" cy="12" r="10"></circle>
                        <polyline points="12 6 12 12 16 14"></polyline>
                    </svg>
                    <span>${test.duration}</span>
                </div>
                <div class="meta-item">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                        <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"></path>
                        <polyline points="14 2 14 8 20 8"></polyline>
                    </svg>
                    <span>${test.type}</span>
                </div>
            </div>
            ${gradeSection}
        </div>
        <div class="test-card-footer">
            ${actionButton}
        </div>
    `;

    return div;
}

function formatArabicDate(dateString) {
    const date = new Date(dateString);
    const options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
    return date.toLocaleDateString('ar-SA', options);
}

// Modal Functions
function setupModal() {
    const modal = document.getElementById('test-modal');
    const closeBtn = document.getElementById('modal-close');

    closeBtn.onclick = () => modal.style.display = 'none';

    window.onclick = (e) => {
        if (e.target === modal) {
            modal.style.display = 'none';
        }
    };
}

function openTestModal(test) {
    const modal = document.getElementById('test-modal');

    // Status badge
    const statusConfig = {
        active: { badge: 'متاح الآن', class: 'active-badge' },
        upcoming: { badge: 'قادم', class: 'upcoming-badge' },
        completed: { badge: 'مكتمل', class: 'completed-badge' },
        missed: { badge: 'فائت', class: 'missed-badge' }
    };

    const config = statusConfig[test.status];
    document.getElementById('modal-badge').textContent = config.badge;
    document.getElementById('modal-badge').className = 'modal-badge ' + config.class;

    // Basic info
    document.getElementById('modal-title').textContent = test.title;
    document.getElementById('modal-subject').textContent = test.subject + ' (' + test.subjectCode + ')';
    document.getElementById('modal-type').textContent = test.type;
    document.getElementById('modal-date').textContent = formatArabicDate(test.date);
    document.getElementById('modal-time').textContent = test.dueTime;
    document.getElementById('modal-duration').textContent = test.duration;

    // Grade (for completed tests)
    const gradeContainer = document.getElementById('modal-grade-container');
    if (test.status === 'completed' && test.grade !== undefined) {
        gradeContainer.style.display = 'block';
        document.getElementById('modal-grade').textContent = `${test.grade}/${test.totalMarks} (${test.percentage}%)`;
    } else {
        gradeContainer.style.display = 'none';
    }

    // Instructions
    const instructionsSection = document.getElementById('modal-instructions');
    const instructionsList = document.getElementById('modal-instructions-list');
    if (test.instructions && test.instructions.length > 0) {
        instructionsSection.style.display = 'block';
        instructionsList.innerHTML = test.instructions.map(i => `<li>${i}</li>`).join('');
    } else {
        instructionsSection.style.display = 'none';
    }

    // Footer action
    const footer = document.getElementById('modal-footer');
    if (test.status === 'active') {
        footer.innerHTML = `<button class="modal-action-btn start-btn" onclick="startTest(${test.id})">بدء الاختبار الآن</button>`;
    } else if (test.status === 'completed') {
        footer.innerHTML = `<button class="modal-action-btn result-btn" onclick="viewResults(${test.id})">عرض تفاصيل النتيجة</button>`;
    } else if (test.status === 'missed') {
        footer.innerHTML = `<button class="modal-action-btn missed-action" onclick="contactTeacher(${test.id})">تواصل مع المعلم</button>`;
    } else {
        footer.innerHTML = `<button class="modal-action-btn upcoming-action" disabled>الاختبار لم يبدأ بعد</button>`;
    }

    modal.style.display = 'flex';
}

// Action Functions
function startTest(id) {
    const test = testsData.find(t => t.id === id);
    if (test) {
        if (confirm(`هل أنت مستعد لبدء اختبار "${test.title}"؟\n\nالمدة: ${test.duration}\nالدرجة الكلية: ${test.totalMarks}`)) {
            alert('جاري تحميل الاختبار... سيتم توجيهك لصفحة الأسئلة.');
            // In real app: window.location.href = `exam.html?id=${id}`;
        }
    }
}

function viewResults(id) {
    const test = testsData.find(t => t.id === id);
    if (test) {
        alert(`نتيجة اختبار: ${test.title}\n\nالدرجة: ${test.grade}/${test.totalMarks}\nالنسبة المئوية: ${test.percentage}%\n\nتم التسليم: ${test.submittedDate}${test.feedback ? '\n\nملاحظات المعلم: ' + test.feedback : ''}`);
    }
}

function contactTeacher(id) {
    const test = testsData.find(t => t.id === id);
    if (test) {
        alert(`سيتم إرسال طلب إعادة الاختبار لمعلم مادة ${test.subject}\n\nسبب الغياب: ${test.missedReason || 'غير محدد'}\n\nسيتم إشعارك بالرد قريباً.`);
    }
}
