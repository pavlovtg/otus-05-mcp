"""E2E-тесты MCP resources: resources/templates/list, api://{name}."""
import httpx
from conftest import mcp_request, parse_sse_response


def list_resources(client: httpx.Client) -> dict:
    payload = mcp_request("resources/templates/list", {}, request_id=2)
    headers = {"Accept": "application/json, text/event-stream"}
    response = client.post("/", json=payload, headers=headers)
    response.raise_for_status()
    return parse_sse_response(response)


def read_resource(client: httpx.Client, uri: str) -> dict:
    payload = mcp_request("resources/read", {"uri": uri}, request_id=3)
    headers = {"Accept": "application/json, text/event-stream"}
    response = client.post("/", json=payload, headers=headers)
    response.raise_for_status()
    return parse_sse_response(response)


def test_resources_list_returns_contracts(mcp_client: httpx.Client) -> None:
    """resources/templates/list должен вернуть список ресурсов api://."""
    result = list_resources(mcp_client)
    assert "result" in result, f"Unexpected response: {result}"
    resources = result["result"]["resourceTemplates"]
    assert len(resources) > 0, "resources/templates/list returned empty list"
    uri_templates = [r["uriTemplate"] for r in resources]
    assert any("api://" in t for t in uri_templates), f"No api:// resource templates found: {uri_templates}"


def test_resource_get_returns_yaml(mcp_client: httpx.Client) -> None:
    """api://{name} должен вернуть YAML-содержимое контракта."""
    result = read_resource(mcp_client, "api://Analytics.DataExporter.V1.yaml")
    assert "result" in result, f"Unexpected response: {result}"
    contents = result["result"]["contents"]
    assert len(contents) > 0, "Resource returned empty contents"
    text = contents[0]["text"]
    # YAML должен содержать openapi или info
    assert "openapi" in text.lower() or "info" in text.lower(), f"Content doesn't look like YAML: {text[:200]}"
