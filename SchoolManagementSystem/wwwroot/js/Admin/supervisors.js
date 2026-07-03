        const viewModal = document.getElementById('viewSupervisorModal');
        const attendanceModal = document.getElementById('attendanceModal');
    
        // أزرار عرض البيانات
        const viewBtns = document.querySelectorAll('.view-btn');
        const attendanceBtns = document.querySelectorAll('.attendance-btn');
    
        // إغلاق 
        function closeAllModals() {
            viewModal.style.display = 'none';
            attendanceModal.style.display = 'none';
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
                const supervisor = this.getAttribute('data-supervisor');
                const number = this.getAttribute('data-number');
                const phone = this.getAttribute('data-phone');
                const specialization = this.getAttribute('data-specialization');
                const attendance = this.getAttribute('data-attendance');
                const evaluation = this.getAttribute('data-evaluation');
            
                document.getElementById('viewSupervisorName').value = supervisor;
                document.getElementById('viewSupervisorNumber').value = number;
                document.getElementById('viewSupervisorPhone').value = phone;
                document.getElementById('viewSupervisorEvaluation').value = evaluation + '%';
            
                viewModal.style.display = 'flex';
            });
        });
    
       // قائمة الحضور
attendanceBtns.forEach(btn => {
    btn.addEventListener('click', function () {

        const supervisor = this.getAttribute('data-supervisor');
        const attendancePercent = parseInt(this.getAttribute('data-attendance'));

        // الأيام اللي غاب فيها
        const absentDays =
    this.getAttribute('data-absent-days')
    ?.split(',')
    .filter(day => day.trim() !== '') || [];

        document.getElementById('attendanceSupervisorName').innerHTML =
            `<i class="fas fa-user"></i> حضور ${supervisor}`;

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


    
        window.addEventListener('click', function(event) {
            if (event.target === viewModal) viewModal.style.display = 'none';
            if (event.target === attendanceModal) attendanceModal.style.display = 'none';
        });
    
       
    
        //  التاريخ الحالي
        const dateElement = document.getElementById('currentDate');
        if (dateElement) {
            const now = new Date();
            const options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
            dateElement.textContent = now.toLocaleDateString('ar-EG', options);
        }
