"""E2E-тесты MCP resources: resources/list, api://{name}."""
import httpx
import pytest
from conftest import initialize_session, mcp_request


@pytest.fixture(scope="module")
def session(mcp_client: httpx.Client) -> str | None:
    return initialize_session(mcp_client)


def list_resources(client: httpx.Client, session_id: str | None) -> dict:
    payload = mcp_request("resources/list", {}, request_id=2)
    headers = {"Accept": "application/json, text/event-stream"}
    if session_id:
        headers["mcp-session-id"] = session_id
    response = client.post("/", json=payload, headers=headers)
    response.raise_for_status()
    return response.json()


def read_resource(client: httpx.Client, session_id: str | None, uri: str) -> dict:
    payload = mcp_request("resources/read", {"uri": uri}, request_id=3)
    headers = {"Accept": "application/json, text/event-stream"}
    if session_id:
        headers["mcp-session-id"] = session_id
    response = client.post("/", json=payload, headers=headers)
    response.raise_for_status()
    return response.json()


def test_resources_list_returns_contracts(mcp_client: httpx.Client, session: str | None) -> None:
    """resources/list должен вернуть список ресурсов api://."""
    result = list_resources(mcp_client, session)
    assert "result" in result, f"Unexpected response: {result}"
    resources = result["result"]["resources"]
    assert len(resources) > 0, "resources/list returned empty list"
    uris = [r["uri"] for r in resources]
    assert any(uri.startswith("api://") for uri in uris), f"No api:// resources found: {uris}"


def test_resource_get_returns_yaml(mcp_client: httpx.Client, session: str | None) -> None:
    """api://{name} должен вернуть YAML-содержимое контракта."""
    # Сначала получаем список ресурсов
    list_result = list_resources(mcp_client, session)
    resources = list_result["result"]["resources"]
    assert len(resources) > 0, "No resources available"

    # Берём первый ресурс
    first_uri = resources[0]["uri"]
    result = read_resource(mcp_client, session, first_uri)
    assert "result" in result, f"Unexpected response: {result}"
    contents = result["result"]["contents"]
    assert len(contents) > 0, "Resource returned empty contents"
    text = contents[0]["text"]
    # YAML должен содержать openapi или info
    assert "openapi" in text.lower() or "info" in text.lower(), f"Content doesn't look like YAML: {text[:200]}"
