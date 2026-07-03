/**
 * Global Child Selection Logic for Parent Dashboard
 * Handles syncing selected child across all Parent pages via localStorage
 */

// Initialize selection on page load
document.addEventListener('DOMContentLoaded', function () {
    syncUIWithSelection();

    // Listen for changes on all child-select and student-select dropdowns
    const childSelect = document.getElementById('child-select');
    if (childSelect) {
        childSelect.addEventListener('change', function () {
            localStorage.setItem('selectedChild', this.value);
        });
    }

    const studentSelect = document.getElementById('student-select');
    if (studentSelect) {
        studentSelect.addEventListener('change', function () {
            localStorage.setItem('selectedChild', this.value);
            syncUIWithSelection();
        });
    }
});

// Sync UI elements with the current value in localStorage
function syncUIWithSelection() {
    let savedChild = localStorage.getItem('selectedChild');

    // Default to first child if nothing is saved
    if (!savedChild) {
        // Try to get the first child card's ID
        const firstCard = document.querySelector('.child-card[data-child-id]');
        if (firstCard) {
            savedChild = firstCard.getAttribute('data-child-id');
        }
        if (savedChild) {
            localStorage.setItem('selectedChild', savedChild);
        }
    }

    // Update all dropdowns on the page
    const allSelects = document.querySelectorAll('#child-select, #student-select');
    allSelects.forEach(function (el) {
        // Check if the value exists in the dropdown options
        const optionExists = Array.from(el.options).some(function (opt) {
            return opt.value === savedChild;
        });
        if (optionExists) {
            el.value = savedChild;
        }
    });

    // Highlight cards on Index page using data-child-id attribute
    const cards = document.querySelectorAll('.child-card');
    cards.forEach(function (card) {
        card.classList.remove('selected');
        var childId = card.getAttribute('data-child-id');
        if (childId && childId === savedChild) {
            card.classList.add('selected');
        }
    });
}

// Global function for card clicks (Index page)
// Saves selection and optionally redirects
function selectChild(childId, performanceUrl) {
    localStorage.setItem('selectedChild', childId.toString());
    syncUIWithSelection();
    if (performanceUrl) {
        window.location.href = performanceUrl + '?child=' + childId;
    }
}

// Global function for dropdown changes (Data pages)
function changeChild() {
    var childSelect = document.getElementById('child-select');
    if (childSelect) {
        localStorage.setItem('selectedChild', childSelect.value);
        syncUIWithSelection();
    }
}

// Global function for footer dropdown changes (Index page)
function syncSelection(childId) {
    localStorage.setItem('selectedChild', childId.toString());
    syncUIWithSelection();
}
