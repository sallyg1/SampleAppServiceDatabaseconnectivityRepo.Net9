<?php
function pima_employee_form() {
    ob_start();
    ?>
    <form method="POST" action="">
        <?php wp_nonce_field('pima_create_employee', 'pima_nonce'); ?>
        <label>First Name: <input type="text" name="first_name" required></label><br>
        <label>Last Name: <input type="text" name="last_name" required></label><br>
        <label>Email: <input type="email" name="email" required></label><br>
        <label>Phone: <input type="text" name="phone"></label><br>
        <label>Department: <input type="text" name="department" required></label><br>
        <label>Job Title: <input type="text" name="job_title" required></label><br>
        <label>Hire Date: <input type="date" name="hire_date" required></label><br>
        <label>Salary: <input type="number" step="0.01" name="salary" required></label><br>
        <label>Active: <input type="checkbox" name="is_active" value="1" checked></label><br>
        <button type="submit" name="pima_submit">Create Employee</button>
    </form>
    <?php
    return ob_get_clean();
}
add_shortcode('employee_form', 'pima_employee_form');


Handle the form submission and call the API:


<?php
function pima_handle_employee_submission() {
    if (!isset($_POST['pima_submit'])) {
        return;
    }

    // Verify nonce for CSRF protection
    if (!isset($_POST['pima_nonce']) || !wp_verify_nonce($_POST['pima_nonce'], 'pima_create_employee')) {
        wp_die('Security check failed.');
    }

    $api_url = 'https://your-api-domain.com/api/database/employees';

    $employee_data = [
        'firstName'  => sanitize_text_field($_POST['first_name']),
        'lastName'   => sanitize_text_field($_POST['last_name']),
        'email'      => sanitize_email($_POST['email']),
        'phone'      => sanitize_text_field($_POST['phone'] ?? ''),
        'department' => sanitize_text_field($_POST['department']),
        'jobTitle'   => sanitize_text_field($_POST['job_title']),
        'hireDate'   => sanitize_text_field($_POST['hire_date']) . 'T00:00:00Z',
        'salary'     => floatval($_POST['salary']),
        'isActive'   => isset($_POST['is_active']),
    ];

    $response = wp_remote_post($api_url, [
        'timeout' => 30,
        'headers' => [
            'Content-Type' => 'application/json',
            'Accept'       => 'application/json',
        ],
        'body' => wp_json_encode($employee_data),
    ]);

    if (is_wp_error($response)) {
        error_log('API request failed: ' . $response->get_error_message());
        return;
    }

    $status_code = wp_remote_retrieve_response_code($response);

    if ($status_code === 201) {
        // Success — redirect or show message
        wp_redirect(add_query_arg('employee_created', '1', wp_get_referer()));
        exit;
    } else {
        error_log('API returned status ' . $status_code . ': ' . wp_remote_retrieve_body($response));
    }
}
add_action('init', 'pima_handle_employee_submission');
