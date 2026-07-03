function saveTeacherAttendance() {
    const absenceDate = $('#absenceDate').val();
    const btn = $('.btn-save');

    const absenceData = {
        AbsenceDate: absenceDate,
        Users: []
    };

    // Collect data from each row
    $('.teacher-row').each(function () {
        const row = $(this);
        absenceData.Users.push({
            UserId: parseInt(row.find('.user-id').val()),
            IsAbsent: row.find('.absent-check').is(':checked'),
            Reason: row.find('.reason-input').val() || ""
        });
    });

    // Basic Validation
    if (absenceData.Users.length === 0) {
        alert("لا يوجد معلمين مسجلين");
        return;
    }

    // UI Feedback
    btn.prop('disabled', true).text('جاري الحفظ...');

    $.ajax({
        url: '/Supervisor/SaveTeachersAbsence',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(absenceData),
        success: function (response) {
            if (response.success) {
                alert(response.message);
                location.reload();
            } else {
                alert(response.message || "حدث خطأ أثناء الحفظ");
                btn.prop('disabled', false).html('<i class="fas fa-cloud-upload-alt"></i> حفظ وإرسال الغياب');
            }
        },
        error: function () {
            alert("فشل الاتصال بالسيرفر");
            btn.prop('disabled', false).html('<i class="fas fa-cloud-upload-alt"></i> حفظ وإرسال الغياب');
        }
    });
}