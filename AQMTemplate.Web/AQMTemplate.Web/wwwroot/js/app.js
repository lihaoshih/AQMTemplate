window.addClass = (elementId, className) => {
    const element = document.getElementById(elementId);
    if (element) {
        element.classList.add(className);
    }
};

window.removeClass = (elementId, className) => {
    const element = document.getElementById(elementId);
    if (element) {
        element.classList.remove(className);
    }
};

window.toggleClass = (elementId, className) => {
    const element = document.getElementById(elementId);
    if (element) {
        element.classList.toggle(className);
    }
};

window.confirmDialog = (message) => {
    return confirm(message);
};

window.sidebarControl = {
    toggle: () => {
        const sidebar = document.querySelector('.sidebar');
        if (sidebar) {
            sidebar.classList.toggle('collapsed');
        }
    },

    expand: () => {
        const sidebar = document.querySelector('.sidebar');
        if (sidebar) {
            sidebar.classList.remove('collapsed');
        }
    },

    collapse: () => {
        const sidebar = document.querySelector('.sidebar');
        if (sidebar) {
            sidebar.classList.add('collapsed');
        }
    }
};

window.localStorage = {
    setItem: (key, value) => {
        localStorage.setItem(key, value);
    },

    getItem: (key) => {
        return localStorage.getItem(key);
    },

    removeItem: (key) => {
        localStorage.removeItem(key);
    },

    clear: () => {
        localStorage.clear();
    }
};

document.addEventListener('DOMContentLoaded', function () {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    const siderbar = document.querySelector('.sidebar');
    let mouseLeaveTimer;

    if (sidebar) {
        sidebar.addEventListener('mouseenter', function () {
            if (mouseLeaveTimer) {
                clearTimeout(mouseLeaveTimer);
            }

            if (sidebar.classList.contains('collapsed')) {
                sidebar.classList.remove('collapsed');
            }
        });

        sidebar.addEventListener('mouseLeave', function () {
            mouseLeaveTimer = setTimeout(() => {
                if (!sidebar.classList.contains('collapsed')) {
                    sidebar.classList.add('collapsed');
                }
            }, 500);
        });
    }
});

window.addEventListener('error', function (e) {
    console.error('Global error:', e.error);
});

window.addEventListener('unhandledrejection', function (e) {
    console.error('Unhandled promise rejection:', e.reason);
});
