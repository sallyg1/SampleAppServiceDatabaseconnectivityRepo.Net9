<?php
$api_url = 'https://your-api-domain.com/api/database/employees';

$response = wp_remote_get($api_url, [
    'timeout' => 30,
    'headers' => [
        'Accept' => 'application/json',
    ],
]);

if (is_wp_error($response)) {
    error_log('API request failed: ' . $response->get_error_message());
    return;
}

$status_code = wp_remote_retrieve_response_code($response);
$body = wp_remote_retrieve_body($response);

if ($status_code === 200) {
    $employees = json_decode($body, true);
    // Use $employees array
    foreach ($employees as $employee) {
        echo esc_html($employee['firstName']) . ' ' . esc_html($employee['lastName']) . '<br>';
    }
} else {
    error_log("API returned status $status_code: $body");
}