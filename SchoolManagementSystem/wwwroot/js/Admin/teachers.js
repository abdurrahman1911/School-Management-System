        const viewModal = document.getElementById('viewTeacherModal');
        const attendanceModal = document.getElementById('attendanceModal');
        const subjectsModal = document.getElementById('subjectsModal');
    
        // أزرار عرض البيانات
        const viewBtns = document.querySelectorAll('.view-btn');
        const attendanceBtns = document.querySelectorAll('.attendance-btn');
        const subjectsBtns = document.querySelectorAll('.grades-btn');
    
        // إغلاق 
        function closeAllModals() {
            viewModal.style.display = 'none';
            attendanceModal.style.display = 'none';
            subjectsModal.style.display = 'none';
        }
    
       const closeIds = [
    'closeViewModal',
    'closeViewBtn',
    'closeAttendanceModal',
    'closeAttendanceBtn',
    'closeSubjectsModal',
    'closeSubjectsBtn'
];

closeIds.forEach(id => {
    const el = document.getElementById(id);
    if (el) {
        el.addEventListener('click', closeAllModals);
    }
});
    
        // قائمة   البيانات 
        viewBtns.forEach(btn => {
            btn.addEventListener('click', function() {
                const teacher = this.getAttribute('data-teacher');
                const number = this.getAttribute('data-number');
                const phone = this.getAttribute('data-phone');
                const specialization = this.getAttribute('data-specialization');
                const attendance = this.getAttribute('data-attendance');
                const evaluation = this.getAttribute('data-evaluation');
            
                document.getElementById('viewTeacherName').value = teacher;
                document.getElementById('viewTeacherNumber').value = number;
                document.getElementById('viewTeacherPhone').value = phone;
                document.getElementById('viewTeacherSpecialization').value = specialization;
                document.getElementById('viewTeacherEvaluation').value = evaluation + '%';
            
                viewModal.style.display = 'flex';
            });
        });
    
       // قائمة الحضور
attendanceBtns.forEach(btn => {
    btn.addEventListener('click', function () {

        const teacher = this.getAttribute('data-teacher');
        const attendancePercent = parseInt(this.getAttribute('data-attendance'));

        // الأيام اللي غاب فيها
        const absentDays =
    this.getAttribute('data-absent-days')
    ?.split(',')
    .filter(day => day.trim() !== '') || [];

        document.getElementById('attendanceTeacherName').innerHTML =
            `<i class="fas fa-user"></i> حضور ${teacher}`;

        const days = ['الأحد', 'الإثنين', 'الثلاثاء', 'الأربعاء', 'الخميس'];

        let daysHTML = '';

        days.forEach(day => {

            const isAbsent = absentDays.includes(day);

            daysHTML += `
                <div class="day-attendance">
                    <div class="day-name">${day}</div>

                    <div class="att-status ${isAbsent ? 'absent' : 'present'}">
                        ${isAbsent ? 'غياب' : 'حضور'}
                    </div>
                </div>
            `;
        });

      daysHTML += `
    <div class="absence-count-full">
        عدد أيام الغياب: ${absentDays.length}
    </div>
`;

        document.getElementById('attendanceDays').innerHTML = daysHTML;

        attendanceModal.style.display = 'flex';
    });
});


       subjectsBtns.forEach(btn => {

    btn.addEventListener('click', function () {

        const teacher = this.getAttribute('data-teacher');

        const subjects =
            this.getAttribute('data-subjects')
            ?.split(',') || [];

        document.getElementById('subjectsTeacherName').innerHTML =
            `<i class="fas fa-chalkboard-user"></i> المواد التي يدرسها ${teacher}`;

        let subjectsHTML = '';

        subjects.forEach(subject => {

            subjectsHTML += `
                <div class="subject-item">
                    <span class="subject-name">${subject}</span>
                </div>
            `;
        });

        document.getElementById('subjectsList').innerHTML = subjectsHTML;

        subjectsModal.style.display = 'flex';
    });
});
    
        window.addEventListener('click', function(event) {
            if (event.target === viewModal) viewModal.style.display = 'none';
            if (event.target === attendanceModal) attendanceModal.style.display = 'none';
            if (event.target === subjectsModal) subjectsModal.style.display = 'none';
        });
    
       
    
        //  التاريخ الحالي
        const dateElement = document.getElementById('currentDate');
        if (dateElement) {
            const now = new Date();
            const options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
            dateElement.textContent = now.toLocaleDateString('ar-EG', options);
        }
