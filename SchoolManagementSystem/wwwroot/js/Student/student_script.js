// Student Portal JavaScript
document.addEventListener('DOMContentLoaded', function() {

/* /////////////////////////////// sidebar for all pages ///////////////////////////////////////////////////////// */
   
    // Navigation active state handling (sidebar active page)
    const navItems = document.querySelectorAll('.nav-item');
    
    navItems.forEach(item => {
        item.addEventListener('click', function(e) {
            // e.preventDefault();
            
            // Remove active class from all items
            navItems.forEach(nav => nav.classList.remove('active'));
            
            // Add active class to clicked item
            this.classList.add('active');
        });
    });


    // Logout button functionality
    const logoutBtn = document.querySelector('.logout-btn');
    
    if (logoutBtn) {
        logoutBtn.addEventListener('click', function() {
            // Simple logout confirmation
            const confirmLogout = confirm('هل تريد تسجيل الخروج؟');
            if (confirmLogout) {
                // alert('تم تسجيل الخروج بنجاح');
                window.location.href = '../../login.html';
            }
        });
    }


    // Table row hover effect enhancement
    const tableRows = document.querySelectorAll('.schedule-table tbody tr');
    
    tableRows.forEach(row => {
        row.addEventListener('mouseenter', function() {
            this.style.cursor = 'pointer';
        });
    });


    // Console log for debugging
    console.log('Student Portal loaded successfully');


/* ////////////////////////////// grades page ////////////////////////////////////////////////////////// */

    // Only run grades-related code if on the grades page (i.e. #levelSelector exists)
    const levelSelector = document.getElementById('levelSelector');
    if (levelSelector) {

        // Grades data for different exam periods (only for trying, we will get the data from the database)
        const gradesData = {
            midterm1: {
                result: 'ناجح',
                grades: [
                    { subject: 'التربية الاسلامية', grade: 27.5 },
                    { subject: 'اللغة العربية', grade: 26 },
                    { subject: 'اللغة الانجليزية', grade: 28.5 },
                    { subject: 'الرياضيات', grade: 30 },
                    { subject: 'الكيمياء', grade: 27 },
                    { subject: 'الاحياء', grade: 28 },
                    { subject: 'الفيزياء', grade: 25.5 },
                    { subject: 'تكنولوجيا المعلومات', grade: 30 }
                ]
            },
            final1: {
                result: 'ناجح',
                grades: [
                    { subject: 'التربية الاسلامية', grade: 29 },
                    { subject: 'اللغة العربية', grade: 27.5 },
                    { subject: 'اللغة الانجليزية', grade: 30 },
                    { subject: 'الرياضيات', grade: 28 },
                    { subject: 'الكيمياء', grade: 26.5 },
                    { subject: 'الاحياء', grade: 29 },
                    { subject: 'الفيزياء', grade: 27 },
                    { subject: 'تكنولوجيا المعلومات', grade: 30 }
                ]
            },
            midterm2: {
                result: 'ناجح',
                grades: [
                    { subject: 'التربية الاسلامية', grade: 28 },
                    { subject: 'اللغة العربية', grade: 25.5 },
                    { subject: 'اللغة الانجليزية', grade: 29 },
                    { subject: 'الرياضيات', grade: 27 },
                    { subject: 'الكيمياء', grade: 28.5 },
                    { subject: 'الاحياء', grade: 26 },
                    { subject: 'الفيزياء', grade: 28 },
                    { subject: 'تكنولوجيا المعلومات', grade: 29 }
                ]
            },
            final2: {
                result: 'ناجح',
                grades: [
                    { subject: 'التربية الاسلامية', grade: 30 },
                    { subject: 'اللغة العربية', grade: 28 },
                    { subject: 'اللغة الانجليزية', grade: 29.5 },
                    { subject: 'الرياضيات', grade: 29 },
                    { subject: 'الكيمياء', grade: 27 },
                    { subject: 'الاحياء', grade: 30 },
                    { subject: 'الفيزياء', grade: 26.5 },
                    { subject: 'تكنولوجيا المعلومات', grade: 30 }
                ]
            }
        };

        // Student info, this would come from login/session
        const studentInfo = {
            name: 'محمود محمد',
            grade: 11,
            currentLevel: 'midterm1' // Default level based on login info
        };

        // Function to update grades table
        function updateGradesTable(level) {
            const data = gradesData[level];
            const tableBody = document.getElementById('gradesTableBody');
            const resultValue = document.querySelector('.result-value');

            if (!tableBody || !resultValue || !data) return;

            // Update result
            resultValue.textContent = data.result;
            resultValue.className = 'result-value ' + (data.result === 'ناجح' ? 'passed' : 'failed');

            // Update table
            tableBody.innerHTML = data.grades.map(item => `
                <tr>
                    <td>${item.subject}</td>
                    <td>${item.grade}</td>
                </tr>
            `).join('');
        }

        // Set up event listener for level selector
        levelSelector.addEventListener('change', function () {
            updateGradesTable(this.value);
        });

        // Set default level based on student info
        levelSelector.value = studentInfo.currentLevel;
        updateGradesTable(studentInfo.currentLevel);

    } // end grades page guard

    // ==========================================================
    // Mobile Responsiveness: Inject Header and Handle Sidebar
    // ==========================================================
    if (!document.querySelector('.mobile-header')) {
        const container = document.querySelector('.container');
        if (container) {
            const headerHtml = `
                <div class="mobile-header">
                    <span class="logo-text">بوابة الطالب</span>
                    <button class="hamburger-btn" type="button">☰</button>
                </div>
                <div class="sidebar-overlay"></div>
            `;
            container.insertAdjacentHTML('afterbegin', headerHtml);

            const hamburgerBtn = document.querySelector('.hamburger-btn');
            const sidebar = document.querySelector('.sidebar');
            const overlay = document.querySelector('.sidebar-overlay');

            if (hamburgerBtn && sidebar && overlay) {
                hamburgerBtn.addEventListener('click', () => {
                    sidebar.classList.toggle('active');
                    overlay.classList.toggle('active');
                });

                overlay.addEventListener('click', () => {
                    sidebar.classList.remove('active');
                    overlay.classList.remove('active');
                });
            }
        }
    }
}); // end DOMContentLoaded
