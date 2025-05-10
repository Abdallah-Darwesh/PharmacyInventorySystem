// Sidebar FAB icon toggle
const sidebarToggle = document.getElementById('sidebarToggle');
const sidebarToggleIcon = document.getElementById('sidebarToggleIcon');
const sidebar = document.getElementById('sidebar');
const sidebarOverlay = document.getElementById('sidebarOverlay');
if (sidebarToggle && sidebarToggleIcon && sidebar && sidebarOverlay) {
    sidebarToggle.addEventListener('click', function () {
        const isOpen = sidebar.classList.contains('show');
        setTimeout(() => {
            if (sidebar.classList.contains('show')) {
                sidebarToggleIcon.innerHTML = '<i class="bi bi-x-lg"></i>';
                sidebarToggle.setAttribute('aria-label', 'Close sidebar');
            } else {
                sidebarToggleIcon.innerHTML = '<i class="bi bi-list"></i>';
                sidebarToggle.setAttribute('aria-label', 'Open sidebar');
            }
        }, 100);
    });
    // Also update icon when overlay or close button is used
    sidebarOverlay.addEventListener('click', function () {
        sidebarToggleIcon.innerHTML = '<i class="bi bi-list"></i>';
        sidebarToggle.setAttribute('aria-label', 'Open sidebar');
    });
}

// Edge swipe/drag to open sidebar (mobile)
(function() {
    const sidebar2 = document.getElementById('sidebar');
    const sidebarOverlay2 = document.getElementById('sidebarOverlay');
    const edgeGrabber = document.getElementById('edgeGrabber');
    let touchStartX = null;
    let touchStartY = null;
    let dragging = false;
    function isMobile() {
        return window.innerWidth < 992;
    }
    // Touch start on left edge (open) or sidebar/overlay (close)
    document.addEventListener('touchstart', function(e) {
        if (!isMobile()) return;
        // Open
        if (e.touches[0].clientX < 24 && sidebar2.classList.contains('hide')) {
            touchStartX = e.touches[0].clientX;
            touchStartY = e.touches[0].clientY;
            dragging = 'open';
        }
        // Close
        if ((sidebar2.classList.contains('show') && (e.target.closest('.sidebar') || e.target === sidebarOverlay2))) {
            touchStartX = e.touches[0].clientX;
            touchStartY = e.touches[0].clientY;
            dragging = 'close';
        }
    });
    // Touch move: open or close
    document.addEventListener('touchmove', function(e) {
        if (!isMobile() || !dragging) return;
        const dx = e.touches[0].clientX - touchStartX;
        const dy = Math.abs(e.touches[0].clientY - touchStartY);
        if (dragging === 'open' && dx > 40 && dy < 40) {
            if (sidebar2 && sidebar2.classList.contains('hide')) {
                sidebar2.classList.add('show');
                sidebar2.classList.remove('hide');
                sidebarOverlay2.style.display = 'block';
            }
            dragging = false;
        }
        if (dragging === 'close' && dx < -40 && dy < 40) {
            if (sidebar2 && sidebar2.classList.contains('show')) {
                sidebar2.classList.remove('show');
                sidebar2.classList.add('hide');
                sidebarOverlay2.style.display = 'none';
            }
            dragging = false;
        }
    });
    document.addEventListener('touchend', function() { dragging = false; });
    // Click/tap on grabber toggles sidebar
    if (edgeGrabber) {
        edgeGrabber.addEventListener('click', function() {
            if (!isMobile()) return;
            if (sidebar2.classList.contains('hide')) {
                sidebar2.classList.add('show');
                sidebar2.classList.remove('hide');
                sidebarOverlay2.style.display = 'block';
            } else {
                sidebar2.classList.remove('show');
                sidebar2.classList.add('hide');
                sidebarOverlay2.style.display = 'none';
            }
        });
        // Keyboard accessibility
        edgeGrabber.addEventListener('keydown', function(e) {
            if ((e.key === 'Enter' || e.key === ' ') && isMobile()) {
                if (sidebar2.classList.contains('hide')) {
                    sidebar2.classList.add('show');
                    sidebar2.classList.remove('hide');
                    sidebarOverlay2.style.display = 'block';
                } else {
                    sidebar2.classList.remove('show');
                    sidebar2.classList.add('hide');
                    sidebarOverlay2.style.display = 'none';
                }
            }
        });
    }
})();

// SPA-like navigation for sidebar links
(function() {
    const mainContent = document.querySelector('.main-content');
    const navLinks = document.querySelectorAll('.sidebar .nav-link');
    const sidebar3 = document.getElementById('sidebar');
    const sidebarOverlay3 = document.getElementById('sidebarOverlay');
    if (!mainContent || !navLinks.length) return;
    // Spinner
    function showSpinner() {
        mainContent.innerHTML = '<div style="display:flex;align-items:center;justify-content:center;height:40vh;"><div class="spinner-border text-success" style="width:3rem;height:3rem;" role="status"><span class="visually-hidden">Loading...</span></div></div>';
    }
    // AJAX load
    async function loadPage(url, push=true) {
        showSpinner();
        try {
            const resp = await fetch(url, { headers: { 'X-Requested-With': 'XMLHttpRequest' } });
            if (!resp.ok) throw new Error('Failed to load');
            const html = await resp.text();
            // Try to extract main content
            const temp = document.createElement('div');
            temp.innerHTML = html;
            let newContent = temp.querySelector('.main-content');
            if (!newContent) newContent = temp.querySelector('main') || temp;
            mainContent.innerHTML = newContent.innerHTML;
            if (push) history.pushState({ url }, '', url);
            // Close sidebar on mobile
            if (window.innerWidth < 992 && sidebar3) {
                sidebar3.classList.remove('show');
                sidebar3.classList.add('hide');
                if (sidebarOverlay3) sidebarOverlay3.style.display = 'none';
            }
            // Call page-specific initializer
            if (window.initManagerSupplierOrderPage) window.initManagerSupplierOrderPage();
        } catch (e) {
            mainContent.innerHTML = '<div class="alert alert-danger text-center">Failed to load page.</div>';
        }
    }
    // Intercept nav-link clicks
    navLinks.forEach(link => {
        link.addEventListener('click', function(e) {
            const url = link.getAttribute('href');
            // Skip .no-spa links and all /Identity links to allow full reload
            if (link.classList.contains('no-spa') || (url && url.includes('/Identity'))) {
                return;
            }
            if (!url || url.startsWith('http') || url.startsWith('mailto:') || url.startsWith('tel:')) return;
            // If navigating to Supplier Orders Overview, do a full page reload
            if (url.includes('/Manager/ManagerSupplierOrder/Index')) {
                window.location.href = url;
                return;
            }
            e.preventDefault();
            loadPage(url);
        });
    });
    // Handle browser back/forward
    window.addEventListener('popstate', function(e) {
        if (e.state && e.state.url) {
            loadPage(e.state.url, false);
        }
    });
})();

// Chart.js re-initialization for SPA navigation
window.initSupplierChart = function(chartLabels, chartData) {
    const chartElem = document.getElementById('supplierChart');
    if (!chartElem) return;
    
    // Destroy existing chart if it exists
    if (window.supplierChartInstance) {
        window.supplierChartInstance.destroy();
    }
    
    const ctx = chartElem.getContext('2d');
    window.supplierChartInstance = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: chartLabels,
            datasets: [{
                label: 'Number of Orders',
                data: chartData,
                backgroundColor: 'rgba(67, 160, 71, 0.6)',
                borderColor: 'rgba(67, 160, 71, 1)',
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: { stepSize: 1 }
                }
            },
            plugins: {
                legend: {
                    position: 'top'
                }
            }
        }
    });
}

// After SPA navigation, re-init chart if needed
(function() {
    const mainContent = document.querySelector('.main-content');
    if (!mainContent) return;
    
    const observer = new MutationObserver((mutations) => {
        mutations.forEach((mutation) => {
            if (mutation.type === 'childList' && mutation.addedNodes.length > 0) {
                const chartElem = document.getElementById('supplierChart');
                if (chartElem && window.Chart) {
                    // Get data from the page
                    const chartLabels = window.supplierChartLabels || [];
                    const chartData = window.supplierChartData || [];
                    if (chartLabels.length && chartData.length) {
                        window.initSupplierChart(chartLabels, chartData);
                    }
                }
            }
        });
    });
    
    observer.observe(mainContent, { 
        childList: true, 
        subtree: true 
    });
})();

window.initManagerSupplierOrderPage = function() {
    console.log('initManagerSupplierOrderPage called');
    const chartElem = document.getElementById('supplierChart');
    console.log('chartElem:', chartElem);
    if (chartElem && window.Chart) {
        let chartLabels = [];
        let chartData = [];
        try {
            chartLabels = JSON.parse(chartElem.getAttribute('data-labels') || '[]');
            chartData = JSON.parse(chartElem.getAttribute('data-data') || '[]');
        } catch (e) {
            chartLabels = [];
            chartData = [];
        }
        console.log('chartLabels:', chartLabels, 'chartData:', chartData);
        if (window.supplierChartInstance) {
            window.supplierChartInstance.destroy();
        }
        const ctx = chartElem.getContext('2d');
        window.supplierChartInstance = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: chartLabels,
                datasets: [{
                    label: 'Number of Orders',
                    data: chartData,
                    backgroundColor: 'rgba(67, 160, 71, 0.6)',
                    borderColor: 'rgba(67, 160, 71, 1)',
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    y: {
                        beginAtZero: true,
                        ticks: { stepSize: 1 }
                    }
                },
                plugins: {
                    legend: {
                        position: 'top'
                    }
                }
            }
        });
    }
    // DataTables
    if (window.$ && window.$.fn && window.$.fn.DataTable && $('#ordersTable').length) {
        console.log('Initializing DataTable');
        $('#ordersTable').DataTable({
            dom: 'Bfrtip',
            buttons: [
                'copyHtml5', 'excelHtml5', 'csvHtml5', 'pdfHtml5', 'print'
            ]
        });
    }
}

// Allow normal form submission for all forms on /Identity pages
// This prevents any JS interception or AJAX for Identity forms
// Use capture phase to run before other handlers

document.addEventListener('submit', function(e) {
    if (window.location.pathname.startsWith('/Identity')) {
        return; // Let the browser handle it
    }
    // ... (other SPA/AJAX form logic, if any, goes here) ...
}, true);
