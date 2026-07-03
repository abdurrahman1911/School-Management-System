// Data
const data = {
    primary: [
        { grade: 'الصف الأول', success: 95, failure: 5 },
        { grade: 'الصف الثاني', success: 90, failure: 10 },
        { grade: 'الصف الثالث', success: 92, failure: 8 },
        { grade: 'الصف الرابع', success: 88, failure: 12 },
        { grade: 'الصف الخامس', success: 85, failure: 15 },
        { grade: 'الصف السادس', success: 80, failure: 20 }
    ],
    middle: [
        { grade: 'الصف الاول المتوسط', success: 82, failure: 18 },
        { grade: 'الصف الثاني المتوسط', success: 78, failure: 22 },
        { grade: 'الصف الثالث المتوسط', success: 75, failure: 25 }
    ],
    secondary: [
        { grade: 'الصف الاول الثانوي', success: 85, failure: 15 },
        { grade: 'الصف  الثاني الثانوي', success: 80, failure: 20 },
        { grade: 'الصف  الثالث الثانوي', success: 88, failure: 12 }
    ]
};

// Create the chart
function createChart(stageData) {
    const chartContainer = document.getElementById('successChart');
    if (!chartContainer) return;

    let chartHTML = `
        <div class="chart-bar">
    `;

    const maxHeight = 280;
    const maxValue = Math.max(...stageData.map(item => Math.max(item.success, item.failure)));

    stageData.forEach((item, index) => {
        const successHeight = (item.success / maxValue) * maxHeight;
        const failureHeight = (item.failure / maxValue) * maxHeight;

        chartHTML += `
            <div class="chart-item" data-grade="${item.grade}">
                <div class="bar bar-success" style="height: ${successHeight}px">
                    <span class="chart-value">${item.success}%</span>
                </div>
                <div class="bar bar-failure" style="height: ${failureHeight}px; margin-top: 5px">
                    <span class="chart-value">${item.failure}%</span>
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
            console.log(`تم النقر على ${grade}`);
        });
    });
}

// Create grades boxes
function createGradesBoxes(stageData) {
    const gradesContainer = document.getElementById('gradesList');
    if (!gradesContainer) return;

    let gradesHTML = '';

    stageData.forEach(item => {
        gradesHTML += `
            <div class="grade-box" data-grade="${item.grade}">
                <h4>${item.grade}</h4>
                <div class="stats-row">
                    <div class="stat success">
                        <div>النجاح</div>
                        <strong>${item.success}%</strong>
                    </div>
                    <div class="stat failure">
                        <div>الرسوب</div>
                        <strong>${item.failure}%</strong>
                    </div>
                </div>
            </div>
        `;
    });

    gradesContainer.innerHTML = gradesHTML;

    // Add click handlers for grade boxes
    document.querySelectorAll('.grade-box').forEach(box => {
        box.addEventListener('click', function () {
            const grade = this.dataset.grade;
            console.log(`تم النقر على ${grade}`);
        });
    });
}

// Update data based on stage and month
function updateData() {
    const stageSelect = document.getElementById('stageSelect');
    const monthSelect = document.getElementById('monthSelect');
    const currentStage = document.getElementById('currentStage');

    if (!stageSelect || !monthSelect || !currentStage) return;

    const stage = stageSelect.value;
    const month = monthSelect.value;

    // Update stage title
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

    // Get the appropriate data
    let stageData = data[stage] || [];

    // Update chart and grades boxes
    createChart(stageData);
    createGradesBoxes(stageData);

    // Calculate and log statistics
    if (stageData.length > 0) {
        const totalSuccess = stageData.reduce((sum, item) => sum + item.success, 0);
        const totalFailure = stageData.reduce((sum, item) => sum + item.failure, 0);
        const avgSuccess = Math.round(totalSuccess / stageData.length);
        const avgFailure = Math.round(totalFailure / stageData.length);

        console.log(`📊 ${stageName} - ${monthName}`);
        console.log(`📈 متوسط النجاح: ${avgSuccess}% | متوسط الرسوب: ${avgFailure}%`);
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
            updateData();

            // Remove loading effect
            applyBtn.classList.remove('btn-loading');
            applyBtn.textContent = 'تطبيق الفلتر';

            // Re-enable selects
            if (stageSelect) stageSelect.disabled = false;
            if (monthSelect) monthSelect.disabled = false;
        }, 300);
    });
}

// Add real-time filter preview
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

// Initialize the page
document.addEventListener('DOMContentLoaded', function () {
    updateDate();
    updateData();
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