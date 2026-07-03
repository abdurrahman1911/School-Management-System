// Attendance and absence data
const attendanceData = {
    primary: [
        { grade: 'الصف الأول', present: 95, absent: 5 },
        { grade: 'الصف الثاني', present: 92, absent: 8 },
        { grade: 'الصف الثالث', present: 96, absent: 4 },
        { grade: 'الصف الرابع', present: 94, absent: 6 },
        { grade: 'الصف الخامس', present: 90, absent: 10 },
        { grade: 'الصف السادس', present: 93, absent: 7 }
    ],
    middle: [
        { grade: 'الصف الاول الموسط', present: 88, absent: 12 },
        { grade: 'الصف الثاني المتوسط', present: 85, absent: 15 },
        { grade: 'الصف الثالث المتوسط', present: 92, absent: 8 }
    ],
    secondary: [
        { grade: ' الصف الاول الثانوي', present: 90, absent: 10 },
        { grade: 'الصف الثاني الثانوي', present: 87, absent: 13 },
        { grade: ' الصف الثالث الثانوي ', present: 95, absent: 5 }
    ]
};

// Create attendance chart
function createAttendanceChart(stageData) {
    const chartContainer = document.getElementById('attendanceChart');
    if (!chartContainer) return;

    const maxHeight = 280;
    const maxValue = Math.max(...stageData.map(item => Math.max(item.present, item.absent)));

    let chartHTML = `
        <div class="chart-bar">
    `;

    stageData.forEach((item, index) => {
        const presentHeight = (item.present / maxValue) * maxHeight;
        const absentHeight = (item.absent / maxValue) * maxHeight;

        chartHTML += `
            <div class="chart-item" data-grade="${item.grade}">
                <div class="bar bar-present" style="height: ${presentHeight}px">
                    <span class="chart-value">${item.present}%</span>
                </div>
                <div class="bar bar-absent" style="height: ${absentHeight}px; margin-top: 5px">
                    <span class="chart-value">${item.absent}%</span>
                </div>
                <div class="chart-label">${item.grade}</div>
            </div>
        `;
    });

    chartHTML += `</div>`;
    chartContainer.innerHTML = chartHTML;

    // Add click handlers for chart items
    document.querySelectorAll('.chart-item').forEach(item => {
        item.addEventListener('click', function () {
            const grade = this.dataset.grade;
            // يمكن إضافة أي تفاعل آخر هنا بدلاً من النوتيفيكيشن
            console.log(`تم النقر على ${grade}`);
        });
    });
}

// Update data with animation
function updateAttendanceData() {
    const stageSelect = document.getElementById('stageSelect');
    const monthSelect = document.getElementById('monthSelect');
    const currentStage = document.getElementById('currentStage');
    const applyBtn = document.getElementById('applyFiltersBtn');

    if (!stageSelect || !monthSelect || !currentStage) return;

    const stage = stageSelect.value;
    const month = monthSelect.value;

    // Get names for display
    const stageNames = {
        'primary': 'المرحلة الإبتدائية',
        'middle': 'المرحلة المتوسطة',
        'secondary': 'المرحلة الثانوية'
    };
    const monthNames = {
        'january': 'يناير', 'february': 'فبراير', 'march': 'مارس',
        'april': 'أبريل', 'may': 'مايو', 'june': 'يونيو',
        'july': 'يوليو', 'august': 'أغسطس', 'september': 'سبتمبر',
        'october': 'أكتوبر', 'november': 'نوفمبر', 'december': 'ديسمبر'
    };

    const stageName = stageNames[stage];
    const monthName = monthNames[month];

    // Update title with animation
    currentStage.style.opacity = '0';
    setTimeout(() => {
        currentStage.textContent = `${stageName} - ${monthName}`;
        currentStage.style.opacity = '1';
    }, 150);

    // Update chart
    const stageData = attendanceData[stage] || [];
    createAttendanceChart(stageData);

    // Update statistics
    if (stageData.length > 0) {
        const totalPresent = stageData.reduce((sum, item) => sum + item.present, 0);
        const totalAbsent = stageData.reduce((sum, item) => sum + item.absent, 0);
        const avgPresent = Math.round(totalPresent / stageData.length);
        const avgAbsent = Math.round(totalAbsent / stageData.length);

        // يمكن إضافة إحصائيات في الـ console فقط
        console.log(`📊 ${stageName} - ${monthName}`);
        console.log(`📈 متوسط الحضور: ${avgPresent}% | متوسط الغياب: ${avgAbsent}%`);
    }
}

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

// Add simple interactions
function addInteractions() {
    // Menu items interaction
    const menuItems = document.querySelectorAll('.menu-item, .item');
    menuItems.forEach(item => {
        item.addEventListener('click', function () {
            // يمكن إضافة أي تفاعل آخر هنا
            console.log(`تم النقر على: ${this.textContent}`);
        });
    });

    // Month selector interaction
    const monthSelector = document.querySelector('.month-selector');
    if (monthSelector) {
        monthSelector.addEventListener('click', function () {
            console.log('تم النقر على اختيار الشهر');
        });
    }
}

// Handle filter button with loading effect
function setupFilterButton() {
    const applyBtn = document.getElementById('applyFiltersBtn');
    if (!applyBtn) return;

    applyBtn.addEventListener('click', function (e) {
        e.preventDefault();

        // Add loading effect
        this.classList.add('btn-loading');
        this.textContent = 'جاري التطبيق...';

        // Disable selects temporarily
        const stageSelect = document.getElementById('stageSelect');
        const monthSelect = document.getElementById('monthSelect');
        if (stageSelect) stageSelect.disabled = true;
        if (monthSelect) monthSelect.disabled = true;

        // Simulate loading time for smooth animation
        setTimeout(() => {
            updateAttendanceData();

            // Remove loading effect
            applyBtn.classList.remove('btn-loading');
            applyBtn.textContent = 'تطبيق الفلتر';

            // Re-enable selects
            if (stageSelect) stageSelect.disabled = false;
            if (monthSelect) monthSelect.disabled = false;
        }, 300);
    });
}

// Add real-time filter preview (optional)
function setupRealTimePreview() {
    const stageSelect = document.getElementById('stageSelect');
    const monthSelect = document.getElementById('monthSelect');

    if (stageSelect) {
        stageSelect.addEventListener('change', function () {
            const stage = this.value;
            const stageNames = {
                'primary': 'المرحلة الإبتدائية',
                'middle': 'المرحلة المتوسطة',
                'secondary': 'المرحلة الثانوية'
            };
            console.log(`تم اختيار: ${stageNames[stage]}`);
        });
    }

    if (monthSelect) {
        monthSelect.addEventListener('change', function () {
            const month = this.value;
            const monthNames = {
                'january': 'يناير', 'february': 'فبراير', 'march': 'مارس',
                'april': 'أبريل', 'may': 'مايو', 'june': 'يونيو',
                'july': 'يوليو', 'august': 'أغسطس', 'september': 'سبتمبر',
                'october': 'أكتوبر', 'november': 'نوفمبر', 'december': 'ديسمبر'
            };
            console.log(`تم اختيار شهر: ${monthNames[month]}`);
        });
    }
}

// Page initialization
document.addEventListener('DOMContentLoaded', function () {
    updateDate();
    updateAttendanceData();
    addInteractions();
    setupFilterButton();
    setupRealTimePreview();

    // Add keyboard shortcut (Ctrl+Enter to apply filter)
    document.addEventListener('keydown', function (e) {
        if ((e.ctrlKey || e.metaKey) && e.key === 'Enter') {
            e.preventDefault();
            const applyBtn = document.getElementById('applyFiltersBtn');
            if (applyBtn) {
                applyBtn.click();
            }
        }
    });
});