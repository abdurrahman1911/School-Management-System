


document.addEventListener("DOMContentLoaded", function () {

    const canvas = document.getElementById("performanceChart");
    if (!canvas || typeof Chart === "undefined") return;

    const ctx = canvas.getContext("2d");

    new Chart(ctx, {
        type: "line",
        data: {
            labels: ["السبت", "الأحد", "الإثنين", "الثلاثاء", "الأربعاء", "الخميس"],
            datasets: [{
                data: [65, 70, 80, 85, 90, 95],
                borderColor: "#1e6d92",
                backgroundColor: "rgba(30,109,146,0.15)",
                fill: true,
                tension: 0.4
            }]
        },
        options: {
            responsive: true,
            plugins: { legend: { display: false } },
            scales: { y: { beginAtZero: true, max: 100 } }
        }
    });
});

function addSupervisor() {
    const input = document.getElementById("supervisorName");
    const list = document.getElementById("supervisorsList");
    if (!input || !list) return;

    const name = input.value.trim();
    if (!name) return;

    const li = document.createElement("li");
    li.textContent = name;
    list.appendChild(li);
    input.value = "";
}

    document.getElementById('saveNoteBtn').addEventListener('click', function() {
            const teacher = document.getElementById('teacherSelect').value;
            const note = document.getElementById('noteContent').value;
        
            if (!teacher) {
                return;
            }
            if (!note.trim()) {
                return;
            }
        
            document.getElementById('noteContent').value = '';
            document.getElementById('teacherSelect').value = '';
        });
    